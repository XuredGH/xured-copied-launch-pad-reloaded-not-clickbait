﻿using Hazel;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using System.Collections.Generic;
using System.Linq;

namespace LaunchpadReloaded.Networking;

[RegisterCustomRpc((uint)LaunchpadRpc.PopulateResults)]
public class PopulateResultsRpc : PlayerCustomRpc<LaunchpadReloadedPlugin, PopulateResultsRpc.Data>
{
    public PopulateResultsRpc(LaunchpadReloadedPlugin plugin, uint id) : base(plugin, id)
    {
    }

    public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;

    public readonly struct Data(byte[] votedFor, byte[] voters)
    {
        public readonly byte[] VotedFor = votedFor;
        public readonly byte[] Voters = voters;
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
        var votes = data.VotedFor.Select((t, i) => new CustomVote(data.Voters[i], t)).ToList();

        VotingTypesManager.HandlePopulateResults(votes);
    }
}