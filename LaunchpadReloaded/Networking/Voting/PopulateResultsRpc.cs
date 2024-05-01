using System.Linq;
using Hazel;
using LaunchpadReloaded.Features.Voting;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.Networking.Voting;

[RegisterCustomRpc((uint)LaunchpadRpc.PopulateResults)]
public class PopulateResultsRpc(LaunchpadReloadedPlugin plugin, uint id) : PlayerCustomRpc<LaunchpadReloadedPlugin, CustomVote[]>(plugin, id)
{
    public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

    public override void Write(MessageWriter writer, CustomVote[] votes)
    {
        writer.WritePacked(votes.Length);
        foreach (var t in votes)
        {
            writer.Write(t.Voter);
            writer.Write(t.Suspect);
        }
    }

    public override CustomVote[] Read(MessageReader reader)
    {
        var votes = new CustomVote[reader.ReadPackedInt32()];
        
        for (var i = 0; i < votes.Length; i++)
        {
            votes[i] = new CustomVote(reader.ReadByte(), reader.ReadByte());
        }

        return votes;
    }

    public override void Handle(PlayerControl player, CustomVote[] votes)
    {
        if (AmongUsClient.Instance.HostId != player.OwnerId)
        {
            player.KickForCheating();
            return;
        }
        
        VotingTypesManager.HandlePopulateResults(votes.ToList());
    }
}