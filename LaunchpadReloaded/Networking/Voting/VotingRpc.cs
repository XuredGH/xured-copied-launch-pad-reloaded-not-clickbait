using System.Linq;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;

namespace LaunchpadReloaded.Networking.Voting;
public static class VotingRpc
{
    [MethodRpc((uint)LaunchpadRpc.RemoveVote)]
    public static void RpcRemoveVote(PlayerControl source, byte voterId, byte votedFor)
    {
        if (source.OwnerId != AmongUsClient.Instance.HostId)
        {
            source.KickForCheating();
            return;
        }

        MeetingHud.Instance.playerStates.First(state => state.TargetPlayerId == voterId).UnsetVote();

        if (PlayerControl.LocalPlayer.PlayerId != voterId)
        {
            return;
        }
        
        MeetingHud.Instance.playerStates.First(state => state.TargetPlayerId == votedFor).ThumbsDown.enabled = false;

        if (!AmongUsClient.Instance.AmHost)
        {
            var voteData = PlayerControl.LocalPlayer.GetModifier<VoteData>();
            voteData.VotesRemaining += 1;
            voteData.VotedPlayers.Remove(votedFor);
        }

        foreach (var t in MeetingHud.Instance.playerStates)
        {
            t.voteComplete = false;
        }

        MeetingHud.Instance.SkipVoteButton.voteComplete = false;
        MeetingHud.Instance.SkipVoteButton.gameObject.SetActive(true);
        
    }
}