using System.Collections.Generic;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;

namespace LaunchpadReloaded.Features;


public class GradientColorManager
{
    public static GradientColorManager Instance;
    public int LocalColorId { get; set; }

    public readonly Dictionary<byte, int> Gradients = new();
    
    public GradientColorManager()
    {
        Instance = this;
    }

    [MethodRpc((uint)LaunchpadRPC.SyncGradient)]
    public static void RpcSetGradient(PlayerControl pc, int colorId)
    {
        Instance.Gradients[pc.PlayerId] = colorId;
    }
}