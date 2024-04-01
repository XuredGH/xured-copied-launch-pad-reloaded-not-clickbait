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
    public static GameObject typeText;

    [HarmonyPostfix, HarmonyPatch(typeof(MeetingHud), "Start")]
    public static void AwakePostfix(MeetingHud __instance)
    {
        foreach (LaunchpadPlayer plr in LaunchpadPlayer.GetAllPlayers())
        {
            plr.VotesRemaining = VotingTypesManager.GetVotes();
            plr.VotedPlayers.Clear();
        }

        DragManager.Instance.DraggingPlayers.Clear();

        if (typeText == null)
        {
            typeText = GameObject.Instantiate(__instance.TimerText, __instance.TimerText.transform.parent).gameObject;
            typeText.GetComponent<TextTranslatorTMP>().Destroy();
            typeText.transform.localPosition += new Vector3(0, 0.25f, 0);
            typeText.name = "VoteTypeText";
        }

        TextMeshPro tmp = typeText.GetComponent<TextMeshPro>();
        tmp.text = LaunchpadPlayer.LocalPlayer.VotesRemaining + " votes left";

        __instance.state = MeetingHud.VoteStates.NotVoted;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(MeetingHud), "SetupProceedButton")]
    public static void ProceedPatch(MeetingHud __instance)
    {
        if (typeText) typeText.gameObject.SetActive(false);
    }

    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "CheckForEndVoting")]
    public static bool EndCheck(MeetingHud __instance)
    {
        if (LaunchpadPlayer.GetAllAlivePlayers().All((plr) => plr.VotesRemaining == 0))
        {
            KeyValuePair<byte, int> max = CalculateNumVotes(CalculateVotes()).MaxPair(out bool isTie);
            GameData.PlayerInfo exiled = GameData.Instance.AllPlayers.ToArray().FirstOrDefault((GameData.PlayerInfo v) => !isTie && v.PlayerId == max.Key);
            MeetingHud.VoterState[] array = new MeetingHud.VoterState[__instance.playerStates.Length];
            __instance.RpcVotingComplete(new MeetingHud.VoterState[__instance.playerStates.Length], exiled, isTie);
        }

        return false;
    }

    public static List<CustomVote> CalculateVotes()
    {
        List<CustomVote> votes = new List<CustomVote>();

        foreach (LaunchpadPlayer player in LaunchpadPlayer.GetAllAlivePlayers())
        {
            foreach (byte vote in player.VotedPlayers)
            {
                votes.Add(new CustomVote(player.Player.PlayerId, vote));
            }
        }

        return votes;
    }

    public static Dictionary<byte, int> CalculateNumVotes(List<CustomVote> votes)
    {
        Dictionary<byte, int> dictionary = new Dictionary<byte, int>();

        foreach (byte vote in votes.Select((vote) => vote.VotedFor))
        {
            if (dictionary.TryGetValue(vote, out int num))
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
        if (LaunchpadGameOptions.Instance.AllowVotingForSamePerson.Value) return true;
        return !LaunchpadPlayer.LocalPlayer.VotedPlayers.Contains(suspect);
    }

    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "ForceSkipAll")]
    public static bool SkipAllPatch(MeetingHud __instance)
    {
        Debug.Log("MAN PLEASE JUST CLAL THE FUNCTIONM");
        foreach (LaunchpadPlayer plr in LaunchpadPlayer.GetAllAlivePlayers())
        {
            __instance.CmdCastVote(plr.Player.PlayerId, 253);
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
        __instance.voted = __instance.myPlayer.GetLpPlayer().VotesRemaining == 0;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(DummyBehaviour), nameof(DummyBehaviour.Start))]
    public static void DummyStartPatch(DummyBehaviour __instance)
    {
        if (LaunchpadSettings.Instance.UniqueDummies.Enabled) __instance.myPlayer.RpcSetName(AccountManager.Instance.GetRandomName());
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

        int num2 = 0;

        foreach (CustomVote vote in votes)
        {
            if (vote.VotedFor == 253)
            {
                MeetingHud.Instance.BloopAVoteIcon(GameData.Instance.GetPlayerById(vote.Voter), num2, MeetingHud.Instance.SkippedVoting.transform);
                num2++;
                continue;
            }

            PlayerVoteArea playerVoteArea = MeetingHud.Instance.playerStates[vote.VotedFor];
            playerVoteArea.ClearForResults();
            int num = 0;
            MeetingHud.Instance.BloopAVoteIcon(GameData.Instance.GetPlayerById(vote.Voter), num, playerVoteArea.transform);
            num++;
        }
    }


    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "PopulateResults")]
    public static bool PopulateResultsPatch(MeetingHud __instance)
    {
        if (AmongUsClient.Instance.AmHost)
        {
            List<CustomVote> votes = CalculateVotes();
            byte[] votedFor = votes.Select((vote) => vote.VotedFor).ToArray();
            byte[] voters = votes.Select((vote) => vote.Voter).ToArray();

            Rpc<PopulateResultsRpc>.Instance.Send(new PopulateResultsRpc.Data(votedFor, voters));
        }

        return false;
    }

    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "CastVote")]
    public static bool CastVotePatch(MeetingHud __instance, [HarmonyArgument(0)] byte playerId, [HarmonyArgument(1)] byte suspectIdx)
    {
        LaunchpadPlayer plr = LaunchpadPlayer.GetById(playerId);
        if (plr.VotesRemaining == 0) return false;

        if (PlayerControl.LocalPlayer.PlayerId == playerId)
        {
            SoundManager.Instance.PlaySound(__instance.VoteLockinSound, false, 1f, null);
        }

        if (suspectIdx == 253)
        {
            plr.VotedPlayers.Clear();
            plr.VotesRemaining = 0;
            plr.VotedPlayers.Add(suspectIdx);
        }
        else
        {
            plr.VotesRemaining -= 1;
            plr.VotedPlayers.Add(suspectIdx);
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
        if (AmongUsClient.Instance.AmHost) return true;

        LaunchpadPlayer plr = LaunchpadPlayer.GetById(playerId);
        if (plr.Player.isDummy)
        {
            if (plr.VotesRemaining == 0) return false;

            if (suspectIdx == 253)
            {
                plr.VotedPlayers.Clear();
                plr.VotesRemaining = 0;
                plr.VotedPlayers.Add(suspectIdx);
            }
            else
            {
                plr.VotesRemaining -= 1;
                plr.VotedPlayers.Add(suspectIdx);
            }
        }

        return true;
    }

    [HarmonyPrefix, HarmonyPatch(typeof(MeetingHud), "Confirm")]
    public static bool ConfirmPatch(MeetingHud __instance, [HarmonyArgument(0)] byte suspect)
    {
        for (int i = 0; i < __instance.playerStates.Length; i++)
        {
            PlayerVoteArea playerVoteArea = __instance.playerStates[i];
            playerVoteArea.ClearButtons();
            if (LaunchpadPlayer.LocalPlayer.VotesRemaining == 1 || suspect == 253) playerVoteArea.voteComplete = true;
        }

        if (LaunchpadPlayer.LocalPlayer.VotesRemaining == 1 || suspect == 253)
        {
            __instance.SkipVoteButton.ClearButtons();
            __instance.SkipVoteButton.voteComplete = true;
            __instance.SkipVoteButton.gameObject.SetActive(false);
        }

        __instance.CmdCastVote(PlayerControl.LocalPlayer.PlayerId, suspect);

        TextMeshPro tmp = typeText.GetComponent<TextMeshPro>();
        tmp.text = LaunchpadPlayer.LocalPlayer.VotesRemaining + " votes left";

        return false;
    }
}