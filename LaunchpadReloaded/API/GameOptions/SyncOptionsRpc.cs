﻿using Hazel;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.API.GameOptions;

// METHOD RPC DOESNT WORK WITH THE ARRAYS AND STUFF SO THIS IS HOW WE WILL DO IT FOR NOW
[RegisterCustomRpc((uint) LaunchpadRPC.SyncGameOptions)]
public class SyncOptionsRpc : PlayerCustomRpc<LaunchpadReloadedPlugin, SyncOptionsRpc.Data>
{
    public SyncOptionsRpc(LaunchpadReloadedPlugin plugin, uint id) : base(plugin, id)
    {
    }

    public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

    public readonly struct Data
    {
        public readonly bool[] Toggles;

        public readonly float[] Numbers;

        public Data(bool[] toggles, float[] numbers)
        {
            Toggles = toggles;
            Numbers = numbers;
        }
    }

    public override void Write(MessageWriter writer, Data data)
    {
        writer.WritePacked((uint)data.Toggles.Length);
        foreach (var t in data.Toggles)
        {
            writer.Write(t);
        }
        
        writer.WritePacked((uint)data.Numbers.Length);
        foreach (var n in data.Numbers)
        {
            writer.Write(n);
        }
    }

    public override Data Read(MessageReader reader)
    {
        var toggles = new bool[reader.ReadPackedUInt32()];
        for (var i = 0; i < toggles.Length; i++)
        {
            toggles[i] = reader.ReadBoolean();
        }

        var numbers = new float[reader.ReadPackedUInt32()];
        for (var i = 0; i < numbers.Length; i++)
        {
            numbers[i] = reader.ReadSingle();
        }


        return new Data(toggles, numbers);
    }

    public override void Handle(PlayerControl player, Data data)
    {
        CustomOptionsManager.HandleOptionsSync(data.Toggles,data.Numbers);
    }
}