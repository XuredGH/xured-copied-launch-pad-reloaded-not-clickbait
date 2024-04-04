using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace LaunchpadReloaded.Features.Managers;
public static class VotingTypesManager
{
    public static VotingTypes SelectedType
    {
        get => (VotingTypes)LaunchpadGameOptions.Instance.VotingType.IndexValue;
        set => LaunchpadGameOptions.Instance.VotingType.SetValue((int)value);
    }

    public static readonly byte[] RecommendedVotes =
    [
        1, 1, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5
    ];

    [MethodRpc((uint)LaunchpadRpc.SetVotingType)]
    public static void RpcSetType(GameData lobby, int type) => SetType((VotingTypes)type);
    public static void SetType(VotingTypes type) => SelectedType = type;

    public static int GetDynamicVotes() => (int)Math.Min(RecommendedVotes[Math.Min(Math.Clamp(LaunchpadPlayer.GetAllAlivePlayers().Count(), 0, 15), RecommendedVotes.Length)], LaunchpadGameOptions.Instance.MaxVotes.Value);

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
        return (from player in LaunchpadPlayer.GetAllAlivePlayers() from vote in player.VoteData.VotedPlayers select new CustomVote(player.player.PlayerId, vote)).ToList();
    }

    public static Dictionary<byte, float> GetChancePercents(List<CustomVote> votes)
    {
        var dict = new Dictionary<byte, float>();

        foreach (var pair in CalculateNumVotes(votes))
            dict[pair.Key] = (pair.Value / votes.Count) * 100;

        return dict;
    }

    public static bool UseChance() => SelectedType == VotingTypes.Chance || SelectedType == VotingTypes.Combined;
    public static bool CanVoteMultiple() => SelectedType is VotingTypes.Multiple or VotingTypes.Combined;
    public static byte GetVotedPlayerByChance(List<CustomVote> votes)
    {
        var rand = new Random();
        List<byte> plrs = [.. votes.Select((vote) => vote.VotedFor)];
        return plrs[rand.Next(plrs.Count)];
    }

    public static Dictionary<byte, float> CalculateNumVotes(List<CustomVote> votes)
    {
        var dictionary = new Dictionary<byte, float>();

        foreach (var vote in votes.Select((vote) => vote.VotedFor))
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

    #endregion

    #region Populate results 
    public static void HandlePopulateResults(List<CustomVote> votes)
    {
        MeetingHud.Instance.TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MeetingVotingResults, Il2CppSystem.Array.Empty<Il2CppSystem.Object>());

        var num2 = 0;

        foreach (var vote in votes)
        {
            if (vote.VotedFor == 253)
            {
                MeetingHud.Instance.BloopAVoteIcon(GameData.Instance.GetPlayerById(vote.Voter), num2, MeetingHud.Instance.SkippedVoting.transform);
                num2++;
                continue;
            }

            var playerVoteArea = MeetingHud.Instance.playerStates[vote.VotedFor];
            var num = 0;
            MeetingHud.Instance.BloopAVoteIcon(GameData.Instance.GetPlayerById(vote.Voter), num, playerVoteArea.transform);
            num++;
        }

        var chances = GetChancePercents(votes);
        if (UseChance() || LaunchpadGameOptions.Instance.ShowPercentages.Value)
        {
            var skipText = MeetingHud.Instance.SkippedVoting;
            skipText.GetComponentInChildren<TextTranslatorTMP>().Destroy();

            chances.TryGetValue(253, out var skips);
            skipText.GetComponentInChildren<TextMeshPro>().text += "\n<size=110%>" + Math.Round(skips, 0) + "%</size>";

            foreach (var voteArea in MeetingHud.Instance.playerStates)
            {
                chances.TryGetValue(voteArea.TargetPlayerId, out var val);
                if (voteArea.AmDead || val < 1) continue;

                var text = $"{Math.Round(val, 0)}%";
                var chanceThing = Object.Instantiate(voteArea.LevelNumberText.transform.parent, voteArea.transform).gameObject;
                chanceThing.gameObject.name = "ChanceCircle";
                chanceThing.transform.localPosition = new Vector3(1.2801f, -0.2431f, -2.5401f);
                chanceThing.transform.localScale = new Vector3(0.35f, 0.35f, 1);
                chanceThing.transform.GetChild(0).gameObject.SetActive(false);
                chanceThing.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);

                var tmp = chanceThing.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                tmp.fontSize = 3f;
                tmp.text = text;
                tmp.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
    #endregion

}