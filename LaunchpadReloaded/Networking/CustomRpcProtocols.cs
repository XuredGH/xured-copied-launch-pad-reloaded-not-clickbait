using Reactor.Networking.Attributes;

namespace LaunchpadReloaded.Networking;

public static class CustomRpcProtocols
{
    [MethodRpc((uint)LaunchpadRpc.CustomMurder)]
    public static void CustomMurderPlayer(this GameData data, PlayerControl source, PlayerControl target, bool didSucceed)
    {
        var murderResultFlags = didSucceed ? MurderResultFlags.Succeeded : MurderResultFlags.FailedError;
        var murderResultFlags2 = MurderResultFlags.DecisionByHost | murderResultFlags;
    
        source.MurderPlayer(target, murderResultFlags2);
    }
}