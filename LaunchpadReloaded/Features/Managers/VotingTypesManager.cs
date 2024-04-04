using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace LaunchpadReloaded.Features.Managers;
public static class VotingTypesManager
{
    public static VotingTypes SelectedType = VotingTypes.Classic;

    public static readonly byte[] RecommendedVotes = new byte[]
    {
        1, 1, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5
    };

    [MethodRpc((uint)LaunchpadRPC.SetVotingType)]
    public static void RpcSetType(GameData lobby, int type) => SetType((VotingTypes)type);
    public static void SetType(VotingTypes type) => SelectedType = type;

    public static int GetDynamicVotes() => (int)Math.Min(RecommendedVotes[Math.Min(Math.Clamp(LaunchpadPlayer.GetAllAlivePlayers().Count, 0, 15), RecommendedVotes.Length)], LaunchpadGameOptions.Instance.MaxVotes.Value);

    public static int GetVotes()
    {
        switch (SelectedType)
        {
            case VotingTypes.Combined:
            case VotingTypes.Multiple:
                return (int)((LaunchpadGameOptions.Instance.AllowVotingForSamePerson.Value && LaunchpadGameOptions.Instance.DisableDynamicVoting.Value) ? LaunchpadGameOptions.Instance.MaxVotes.Value :
                    GetDynamicVotes());

            case VotingTypes.Chance:
            case VotingTypes.Classic:
            default:
                return 1;
        }
    }

    #region Vote calculations
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

    public static Dictionary<byte, float> GetChancePercents(List<CustomVote> votes)
    {
        Dictionary<byte, float> dict = new Dictionary<byte, float>();

        foreach (KeyValuePair<byte, float> pair in CalculateNumVotes(votes))
            dict[pair.Key] = (pair.Value / votes.Count) * 100;

        return dict;
    }

    public static byte GetVotedPlayerByChance(List<CustomVote> votes)
    {
        Random rand = new Random();
        List<byte> plrs = [.. votes.Select((vote) => vote.VotedFor)];
        return plrs[rand.Next(plrs.Count)];
    }

    public static Dictionary<byte, float> CalculateNumVotes(List<CustomVote> votes)
    {
        Dictionary<byte, float> dictionary = new Dictionary<byte, float>();

        foreach (byte vote in votes.Select((vote) => vote.VotedFor))
        {
            if (dictionary.TryGetValue(vote, out float num))
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

    #endregion

    #region Populate results 
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
            int num = 0;
            MeetingHud.Instance.BloopAVoteIcon(GameData.Instance.GetPlayerById(vote.Voter), num, playerVoteArea.transform);
            num++;
        }

        Dictionary<byte, float> chances = GetChancePercents(votes);
        if (UseChance() || LaunchpadGameOptions.Instance.ShowPercentages.Value)
        {
            GameObject skipText = MeetingHud.Instance.SkippedVoting;
            skipText.GetComponentInChildren<TextTranslatorTMP>().Destroy();

            chances.TryGetValue(253, out float skips);
            skipText.GetComponentInChildren<TextMeshPro>().text += "\n<size=110%>" + Math.Round(skips, 0) + "%</size>";

            foreach (PlayerVoteArea voteArea in MeetingHud.Instance.playerStates)
            {
                chances.TryGetValue(voteArea.TargetPlayerId, out float val);
                if (voteArea.AmDead || val < 1) continue;

                string text = $"{Math.Round(val, 0)}%";
                GameObject chanceThing = GameObject.Instantiate(voteArea.LevelNumberText.transform.parent, voteArea.transform).gameObject;
                chanceThing.gameObject.name = "ChanceCircle";
                chanceThing.transform.localPosition = new Vector3(1.2801f, -0.2431f, -2.5401f);
                chanceThing.transform.localScale = new Vector3(0.35f, 0.35f, 1);
                chanceThing.transform.GetChild(0).gameObject.SetActive(false);
                chanceThing.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);

                TextMeshPro tmp = chanceThing.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                tmp.fontSize = 3f;
                tmp.text = text;
                tmp.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
    #endregion

    public static bool UseChance() => SelectedType == VotingTypes.Chance || SelectedType == VotingTypes.Combined;
    public static bool CanVoteMultiple() => SelectedType == VotingTypes.Multiple || SelectedType == VotingTypes.Combined;
}