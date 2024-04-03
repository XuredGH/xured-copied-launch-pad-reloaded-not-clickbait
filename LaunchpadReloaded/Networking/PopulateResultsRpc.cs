using Hazel;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using System.Collections.Generic;

namespace LaunchpadReloaded.Networking;

[RegisterCustomRpc((uint)LaunchpadRPC.PopulateResults)]
public class PopulateResultsRpc : PlayerCustomRpc<LaunchpadReloadedPlugin, PopulateResultsRpc.Data>
{
    public PopulateResultsRpc(LaunchpadReloadedPlugin plugin, uint id) : base(plugin, id)
    {
    }

    public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;

    public readonly struct Data
    {
        public readonly byte[] VotedFor;
        public readonly byte[] Voters;
        public Data(byte[] votedFor, byte[] voters)
        {
            VotedFor = votedFor;
            Voters = voters;
        }
    }

    public override void Write(MessageWriter writer, Data data)
    {
        writer.WritePacked((uint)data.VotedFor.Length);
        foreach (var t in data.VotedFor)
        {
            writer.Write(t);
        }

        writer.WritePacked((uint)data.Voters.Length);
        foreach (var n in data.Voters)
        {
            writer.Write(n);
        }
    }

    public override Data Read(MessageReader reader)
    {
        var votedFor = new byte[reader.ReadPackedUInt32()];
        for (var i = 0; i < votedFor.Length; i++)
        {
            votedFor[i] = reader.ReadByte();
        }

        var voters = new byte[reader.ReadPackedUInt32()];
        for (var i = 0; i < voters.Length; i++)
        {
            voters[i] = reader.ReadByte();
        }


        return new Data(votedFor, voters);
    }

    public override void Handle(PlayerControl player, Data data)
    {
        List<CustomVote> votes = new List<CustomVote>();
        for (var i = 0; i < data.VotedFor.Length; i++)
        {
            votes.Add(new CustomVote(data.Voters[i], data.VotedFor[i]));
        }

        VotingTypesManager.HandlePopulateResults(votes);
    }
}