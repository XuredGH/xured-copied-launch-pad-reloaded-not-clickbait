using Hazel;
using LaunchpadReloaded.API.GameOptions;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using UnityEngine;

namespace LaunchpadReloaded.Networking;

[RegisterCustomRpc((uint) LaunchpadRPC.SyncGameOptions)]
public class SyncCustomGameOptionsRpc : PlayerCustomRpc<LaunchpadReloadedPlugin, SyncCustomGameOptionsRpc.Data>
{
    public SyncCustomGameOptionsRpc(LaunchpadReloadedPlugin plugin, uint id) : base(plugin, id)
    {
    }

    public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

    public readonly struct Data
    {
        public readonly bool[] ToggleOptions;
        public readonly float[] NumberOptions;

        public Data(bool[] toggleOptions, float[] numberOptions)
        {
            ToggleOptions = toggleOptions;
            NumberOptions = numberOptions;
        }
    }

    public override void Write(MessageWriter writer, Data data)
    {
        writer.WritePacked((uint)data.ToggleOptions.Length);
        foreach (var toggle in data.ToggleOptions)
        {
            writer.Write(toggle);
        }
        
        writer.WritePacked((uint)data.NumberOptions.Length);
        foreach (var number in data.NumberOptions)
        {
            writer.Write(number);
        }

    }

    public override Data Read(MessageReader reader)
    {
        var len = reader.ReadPackedUInt32();
        var toggles = new bool[len];
        for (var i = 0; i < len; i++)
        {
            toggles[i] = reader.ReadBoolean();
            Debug.LogError(toggles[i]);
        }

        len = reader.ReadPackedUInt32();
        var numbers = new float[len];
        for (var i = 0; i < len; i++)
        {
            numbers[i] = reader.ReadSingle();
            Debug.LogError(numbers[i]);
        }

        return new Data(toggles, numbers);
    }

    public override void Handle(PlayerControl player, Data data)
    {
        CustomGameOptionsManager.ReadSyncOptions(data.ToggleOptions, data.NumberOptions);
    }
}