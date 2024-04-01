using Reactor.Networking.Attributes;

namespace LaunchpadReloaded.Networking;

public static class CustomRpcProtocols
{
    [MethodRpc((uint)LaunchpadRpc.CustomCheckMurder)]
    public static void CustomCheckMurder(this PlayerControl source, PlayerControl target)
    {
        if (!AmongUsClient.Instance.AmHost)
        {
            source.KickForCheating();
            return;
        }
        

        if (AmongUsClient.Instance.IsGameOver || MeetingHud.Instance || 
            !VerifyTarget(target) || !VerifyTarget(source) || !source.Data.Role.CanUseKillButton) 
        {
            GameData.Instance.CustomMurderPlayer(source, target, false);
            return;
        }

        source.isKilling = true;
        GameData.Instance.CustomMurderPlayer(source, target, true);
        
        
    }

    private static bool VerifyTarget(this PlayerControl player)
    {
        var data = player.Data;
        return data is not null && 
               (data.IsDead || data.Disconnected || player.inVent || player.MyPhysics.Animations.IsPlayingEnterVentAnimation() || 
                player.MyPhysics.Animations.IsPlayingAnyLadderAnimation() || player.inMovingPlat);
    }
    
    
    [MethodRpc((uint)LaunchpadRpc.CustomMurder)]
    public static void CustomMurderPlayer(this GameData data, PlayerControl source, PlayerControl target, bool didSucceed)
    {
        var murderResultFlags = didSucceed ? MurderResultFlags.Succeeded : MurderResultFlags.FailedError;
        var murderResultFlags2 = MurderResultFlags.DecisionByHost | murderResultFlags;
        if (AmongUsClient.Instance.AmClient)
        {
            source.MurderPlayer(target, murderResultFlags2);
        }
    }
    
}