using LaunchpadReloaded.Features;
using Reactor.Networking.Attributes;
using System.Linq;

namespace LaunchpadReloaded.Networking;
public static class VotingRpc
{
    [MethodRpc((uint)LaunchpadRpc.RemoveVote)]
    public static void RpcRemoveVote(PlayerControl source, byte voter, byte votedFor)
    {
        if (source.OwnerId != AmongUsClient.Instance.HostId)
        {
            return;
        }

        MeetingHud.Instance.playerStates.First((state) => state.TargetPlayerId == voter).UnsetVote();

        if (PlayerControl.LocalPlayer.PlayerId == voter)
        {
            MeetingHud.Instance.playerStates.First((state) => state.TargetPlayerId == votedFor).ThumbsDown.enabled = false;

            if (!AmongUsClient.Instance.AmHost)
            {
                LaunchpadPlayer.LocalPlayer.VoteData.VotesRemaining += 1;
                LaunchpadPlayer.LocalPlayer.VoteData.VotedPlayers.Remove(votedFor);
            }

            for (int i = 0; i < MeetingHud.Instance.playerStates.Length; i++)
            {
                MeetingHud.Instance.playerStates[i].voteComplete = false;
            }

            MeetingHud.Instance.SkipVoteButton.voteComplete = false;
            MeetingHud.Instance.SkipVoteButton.gameObject.SetActive(true);
        }
    }
}