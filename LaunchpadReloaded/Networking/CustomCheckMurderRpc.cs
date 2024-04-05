using Hazel;
using InnerNet;
using LaunchpadReloaded.API.Roles;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.Networking;

[RegisterCustomRpc((uint)LaunchpadRpc.CustomCheckMurder)]
public class CustomCheckMurderRpc(LaunchpadReloadedPlugin plugin, uint id)
    : PlayerCustomRpc<LaunchpadReloadedPlugin, PlayerControl>(plugin, id)
{
    public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;
    public override void Write(MessageWriter writer, PlayerControl data)
    {
        writer.WriteNetObject(data);
    }

    public override PlayerControl Read(MessageReader reader)
    {
        return reader.ReadNetObject<PlayerControl>();
    }

    private static bool VerifyTarget(PlayerControl player)
    {
        var data = player.Data;
        return data is not null && 
               !(data.IsDead || data.Disconnected || player.inVent || player.MyPhysics.Animations.IsPlayingEnterVentAnimation() || 
                 player.MyPhysics.Animations.IsPlayingAnyLadderAnimation() || player.inMovingPlat);
    }

    private static bool VerifyKiller(PlayerControl player)
    {
        var data = player.Data;
        if (data is null)
        {
            return false;
        }

        if (CustomRoleManager.GetCustomRoleBehaviour(data.RoleType, out var customRole))
        {
            return customRole.CanKill && !data.Disconnected;
        }

        return data.Role.CanUseKillButton && !(data.IsDead || !data.Role.IsImpostor || data.Disconnected);
    }
    
    public override void Handle(PlayerControl source, PlayerControl target)
    {
        if (!AmongUsClient.Instance.AmHost)
        {
            return;
        }
        
        if (AmongUsClient.Instance.IsGameOver || MeetingHud.Instance || 
            !VerifyTarget(target) || !VerifyKiller(source)) 
        {
            GameData.Instance.CustomMurderPlayer(source, target, false);
            return;
        }

        source.isKilling = true;
        GameData.Instance.CustomMurderPlayer(source, target, true);
    }
}