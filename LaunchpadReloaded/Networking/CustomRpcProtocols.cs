using LaunchpadReloaded.Features.Managers;
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
    
    [MethodRpc((uint)LaunchpadRpc.CustomSetColor)]
    public static void CustomSetColor(this GameData data, PlayerControl source, byte colorId, byte gradient)
    {
        source.SetColor(colorId);
        source.SetGradient(gradient);
    }
    
}