using Hazel;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.API.GameOptions;

// METHOD RPC DOESNT WORK WITH THE ARRAYS AND STUFF SO THIS IS HOW WE WILL DO IT FOR NOW
[RegisterCustomRpc((uint)LaunchpadRPC.SyncGameOptions)]
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
        public readonly string[] Strings;

        public Data(bool[] toggles, float[] numbers, string[] strings)
        {
            Toggles = toggles;
            Numbers = numbers;
            Strings = strings;
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

        writer.WritePacked((uint)data.Strings.Length);
        foreach (var n in data.Strings)
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

        var strings = new string[reader.ReadPackedUInt32()];
        for (var i = 0; i < strings.Length; i++)
        {
            strings[i] = reader.ReadString();
        }


        return new Data(toggles, numbers, strings);
    }

    public override void Handle(PlayerControl player, Data data)
    {
        CustomOptionsManager.HandleOptionsSync(data.Toggles, data.Numbers, data.Strings);
    }
}