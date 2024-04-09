using Hazel;
using InnerNet;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.Networking.Murder;

[RegisterCustomRpc((uint)LaunchpadRpc.CustomMurder)]
public class CustomRpcMurder(LaunchpadReloadedPlugin plugin, uint id) : PlayerCustomRpc<LaunchpadReloadedPlugin, CustomMurderData>(plugin, id)
{
    public override RpcLocalHandling LocalHandling => RpcLocalHandling.After; // to prevent host from ending game too early? needs testing
    
    public override void Write(MessageWriter writer, CustomMurderData data)
    {
        writer.WriteNetObject(data.TargetPlayer);
        writer.Write((int)data.MurderResultFlags);
    }

    public override CustomMurderData Read(MessageReader reader)
    {
        return new CustomMurderData(reader.ReadNetObject<PlayerControl>(), (MurderResultFlags)reader.ReadInt32());
    }

    public override void Handle(PlayerControl source, CustomMurderData data)
    {
        if (!source.Data.Role.CanUseKillButton)
        {
            source.KickForCheating();
            return;
        }
        
        if (AmongUsClient.Instance.AmClient)
        {
            source.MurderPlayer(data.TargetPlayer, data.MurderResultFlags);
        }
    }
}