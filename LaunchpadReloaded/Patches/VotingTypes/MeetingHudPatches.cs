using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Utilities;
using Reactor.Networking.Rpc;
using Reactor.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
namespace LaunchpadReloaded.Patches.VotingTypes;

[HarmonyPatch]
public static class MeetingHudPatches
{
    private static GameObject _typeText;

    [HarmonyPostfix, HarmonyPatch(typeof(MeetingHud), "Start")]
    public static void AwakePostfix(MeetingHud __instance)
    {
        foreach (var plr in LaunchpadPlayer.GetAllPlayers())
        {
            plr.VoteData.VotesRemaining = VotingTypesManager.GetVotes();
            plr.VoteData.VotedPlayers.Clear();
        }

        DragManager.Instance.DraggingPlayers.Clear();

        if (_typeText == null)
        {
            _typeText = Object.Instantiate(__instance.TimerText, __instance.TimerText.transform.parent).gameObject;
            _typeText.GetComponent<TextTranslatorTMP>().Destroy();
            _typeText.transform.localPosition += new Vector3(0, 0.25f, 0);
            _typeText.name = "VoteTypeText";
        }

        var tmp = _typeText.GetComponent<TextMeshPro>();
        tmp.text = LaunchpadPlayer.LocalPlayer.VoteData.VotesRemaining + " votes left";

        __instance.state = MeetingHud.VoteStates.NotVoted;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(MeetingHud), "SetupProceedButton")]
    public static void ProceedPatch(MeetingHud __instance)
    {
        if (_typeText)
        {
            _typeText.gameObject.SetActive(false);
        }
    }

    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "CheckForEndVoting")]
    public static bool EndCheck(MeetingHud __instance)
    {
        if (LaunchpadPlayer.GetAllAlivePlayers().Any(plr => plr.VoteData.VotesRemaining > 0))
        {
            return false;
        }
        
        var max = CalculateNumVotes(CalculateVotes()).MaxPair(out var isTie);
        var exiled = GameData.Instance.AllPlayers.ToArray().FirstOrDefault(v => !isTie && v.PlayerId == max.Key);
        var array = new MeetingHud.VoterState[__instance.playerStates.Length];
        __instance.RpcVotingComplete(array, exiled, isTie);

        return false;
    }

    public static List<CustomVote> CalculateVotes()
    {
        return (from player in LaunchpadPlayer.GetAllAlivePlayers() from vote in player.VoteData.VotedPlayers select new CustomVote(player.player.PlayerId, vote)).ToList();
    }

    public static Dictionary<byte, int> CalculateNumVotes(List<CustomVote> votes)
    {
        var dictionary = new Dictionary<byte, int>();

        foreach (var vote in votes.Select(vote => vote.VotedFor))
        {
            if (dictionary.TryGetValue(vote, out var num))
            {
                dictionary[vote] = num + 1;

            }
            else
            {
                dictionary[vote] = 1;
            }
        }

        return dictionary;
    }

    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "Select")]
    public static bool SelectPatch(MeetingHud __instance, [HarmonyArgument(0)] byte suspect)
    {
        if (LaunchpadGameOptions.Instance.AllowVotingForSamePerson.Value)
        {
            return true;
        }

        return !LaunchpadPlayer.LocalPlayer.VoteData.VotedPlayers.Contains(suspect);
    }

    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "ForceSkipAll")]
    public static bool SkipAllPatch(MeetingHud __instance)
    {
        Debug.Log("MAN PLEASE JUST CLAL THE FUNCTIONM");
        foreach (var plr in LaunchpadPlayer.GetAllAlivePlayers())
        {
            __instance.CmdCastVote(plr.player.PlayerId, 253);
        }
        __instance.CheckForEndVoting();

        return false;
    }

    /*    [HarmonyPrefix, HarmonyPatch(typeof(DummyBehaviour), nameof(DummyBehaviour.Update))]
        public static bool DummyUpdatePatch(DummyBehaviour __instance)
        {
            GameData.PlayerInfo data = __instance.myPlayer.Data;
            if (data == null || data.IsDead)
            {
                return false;
            }
            if (MeetingHud.Instance)
            {
                if (__instance.myPlayer.GetLpPlayer().VotesRemaining != 0)
                {
                    __instance.StartCoroutine(__instance.DoVote());
                    return false;
                }
            }

            return false;
        }*/

    [HarmonyPostfix, HarmonyPatch(typeof(DummyBehaviour), nameof(DummyBehaviour.Update))]
    public static void DummyUpdatePatch(DummyBehaviour __instance)
    {
        __instance.voted = __instance.myPlayer.GetLpPlayer().VoteData.VotesRemaining == 0;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(DummyBehaviour), nameof(DummyBehaviour.Start))]
    public static void DummyStartPatch(DummyBehaviour __instance)
    {
        if (LaunchpadSettings.Instance.UniqueDummies.Enabled)
        {
            __instance.myPlayer.RpcSetName(AccountManager.Instance.GetRandomName());
        }
    }

    /*    [HarmonyPostfix, HarmonyPatch(typeof(MeetingHud), "ClearVote")]
        public static void ClearVotePatch(MeetingHud __instance)
        {
            PlayerControl.LocalPlayer.RpcEditVotes(0);
            PlayerControl.LocalPlayer.RpcClearVote();
        }
    */
    public static void HandlePopulateResults(List<CustomVote> votes)
    {
        MeetingHud.Instance.TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MeetingVotingResults, Il2CppSystem.Array.Empty<Il2CppSystem.Object>());

        var num2 = 0;
        var num = 0;

        foreach (var vote in votes)
        {
            if (vote.VotedFor == 253)
            {
                MeetingHud.Instance.BloopAVoteIcon(GameData.Instance.GetPlayerById(vote.Voter), num2, MeetingHud.Instance.SkippedVoting.transform);
                num2++;
                continue;
            }

            var playerVoteArea = MeetingHud.Instance.playerStates[vote.VotedFor];
            playerVoteArea.ClearForResults();
            MeetingHud.Instance.BloopAVoteIcon(GameData.Instance.GetPlayerById(vote.Voter), num, playerVoteArea.transform);
            num++;
        }
    }


    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "PopulateResults")]
    public static bool PopulateResultsPatch(MeetingHud __instance)
    {
        if (!AmongUsClient.Instance.AmHost)
        {
            return false;
        }
        
        var votes = CalculateVotes();
        var votedFor = votes.Select(vote => vote.VotedFor).ToArray();
        var voters = votes.Select(vote => vote.Voter).ToArray();

        Rpc<PopulateResultsRpc>.Instance.Send(new PopulateResultsRpc.Data(votedFor, voters));

        return false;
    }

    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "CastVote")]
    public static bool CastVotePatch(MeetingHud __instance, [HarmonyArgument(0)] byte playerId, [HarmonyArgument(1)] byte suspectIdx)
    {
        var plr = LaunchpadPlayer.GetById(playerId);
        if (plr.VoteData.VotesRemaining == 0)
        {
            return false;
        }

        if (PlayerControl.LocalPlayer.PlayerId == playerId)
        {
            SoundManager.Instance.PlaySound(__instance.VoteLockinSound, false);
        }

        if (suspectIdx == 253)
        {
            plr.VoteData.VotedPlayers.Clear();
            plr.VoteData.VotesRemaining = 0;
            plr.VoteData.VotedPlayers.Add(suspectIdx);
        }
        else
        {
            plr.VoteData.VotesRemaining -= 1;
            plr.VoteData.VotedPlayers.Add(suspectIdx);
        }

        __instance.SetDirtyBit(1U);
        __instance.CheckForEndVoting();
        PlayerControl.LocalPlayer.RpcSendChatNote(playerId, ChatNoteTypes.DidVote);

        //Debug.Log(LaunchpadPlayer.GetById(playerId).Player.Data.PlayerName + " has voted for " + suspectIdx);
        //Debug.Log(LaunchpadPlayer.GetById(playerId).Player.Data.PlayerName + " has voted " + LaunchpadPlayer.GetById(playerId).VotesRemaining + " votes left.");

        return false;
    }

    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "CmdCastVote")]
    public static bool CmdCastVotePatch(MeetingHud __instance, [HarmonyArgument(0)] byte playerId, [HarmonyArgument(1)] byte suspectIdx)
    {
        if (AmongUsClient.Instance.AmHost)
        {
            return true;
        }

        var plr = LaunchpadPlayer.GetById(playerId);
        if (!plr.player.isDummy)
        {
            return true;
        }
        
        if (plr.VoteData.VotesRemaining < 1)
        {
            return false;
        }

        if (suspectIdx == 253)
        {
            plr.VoteData.VotedPlayers.Clear();
            plr.VoteData.VotesRemaining = 0;
            plr.VoteData.VotedPlayers.Add(suspectIdx);
        }
        else
        {
            plr.VoteData.VotesRemaining -= 1;
            plr.VoteData.VotedPlayers.Add(suspectIdx);
        }

        return true;
    }

    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "Confirm")]
    public static bool ConfirmPatch(MeetingHud __instance, [HarmonyArgument(0)] byte suspect)
    {
        for (var i = 0; i < __instance.playerStates.Length; i++)
        {
            var playerVoteArea = __instance.playerStates[i];
            playerVoteArea.ClearButtons();
            if (LaunchpadPlayer.LocalPlayer.VoteData.VotesRemaining == 1 || suspect == 253)
            {
                playerVoteArea.voteComplete = true;
            }
        }

        if (LaunchpadPlayer.LocalPlayer.VoteData.VotesRemaining == 1 || suspect == 253)
        {
            __instance.SkipVoteButton.ClearButtons();
            __instance.SkipVoteButton.voteComplete = true;
            __instance.SkipVoteButton.gameObject.SetActive(false);
        }

        __instance.CmdCastVote(PlayerControl.LocalPlayer.PlayerId, suspect);

        var tmp = _typeText.GetComponent<TextMeshPro>();
        tmp.text = LaunchpadPlayer.LocalPlayer.VoteData.VotesRemaining + " votes left";

        return false;
    }
}