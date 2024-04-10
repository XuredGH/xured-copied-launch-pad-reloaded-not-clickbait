using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;

namespace LaunchpadReloaded.Networking.Murder;

public static class CustomMurderRpc
{
    [MethodRpc((uint)LaunchpadRpc.CustomMurder, LocalHandling = RpcLocalHandling.After, SendImmediately = true)]
    public static void RpcCustomMurder(this PlayerControl source, PlayerControl target, bool didSucceed)
    {
        if (!source.Data.Role.CanUseKillButton)
        {
            source.KickForCheating();
            return;
        }
        
        var murderResultFlags = didSucceed ? MurderResultFlags.Succeeded : MurderResultFlags.FailedError;
        var murderResultFlags2 = MurderResultFlags.DecisionByHost | murderResultFlags;
        
        if (AmongUsClient.Instance.AmClient)
        {
            source.MurderPlayer(target, murderResultFlags2);
            Logger<LaunchpadReloadedPlugin>.Warning(murderResultFlags2);
        }
    }
}