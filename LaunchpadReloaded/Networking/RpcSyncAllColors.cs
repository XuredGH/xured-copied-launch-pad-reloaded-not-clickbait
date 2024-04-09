﻿using System.Collections.Generic;
using Hazel;
using LaunchpadReloaded.Networking.Data;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.Networking;

[RegisterCustomRpc((uint)LaunchpadRpc.SyncAllColors)]
public class RpcSyncAllColors(LaunchpadReloadedPlugin plugin, uint id)
    : PlayerCustomRpc<LaunchpadReloadedPlugin, Dictionary<byte, CustomColorData>>(plugin, id)
{

    public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;
    
    public override void Write(MessageWriter writer, Dictionary<byte, CustomColorData> data)
    {
        writer.Write((byte)data.Count);
        foreach (var var in data)
        {
            writer.Write(var.Key);
            writer.Write(var.Value.ColorId);
            writer.Write(var.Value.GradientId);
        }
    }

    public override Dictionary<byte, CustomColorData> Read(MessageReader reader)
    {
        var data = new Dictionary<byte, CustomColorData>();
        
        for (var i = 0; i < reader.ReadByte(); i++)
        {
            data.Add(reader.ReadByte(), new CustomColorData(reader.ReadByte(), reader.ReadByte()));
        }

        return data;
    }

    public override void Handle(PlayerControl pc, Dictionary<byte, CustomColorData> data)
    {
        if (AmongUsClient.Instance.HostId != pc.OwnerId)
        {
            return;
        }

        foreach (var info in data)
        {
            var player = GameData.Instance.GetPlayerById(info.Key);
            if (player is null || !player.Object)
            {
                return;
            }
            
            Rpc<CustomRpcSetColor>.Instance.Handle(player.Object, new CustomColorData(info.Value.ColorId, info.Value.GradientId));
        }
    }
}