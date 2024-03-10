using System.Collections.Generic;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;

namespace LaunchpadReloaded.Features;


public class GradientColorManager
{
    public static GradientColorManager Instance;
    public int LocalGradientId { get; set; }
    public Dictionary<byte, int> Gradients = new();
    
    public GradientColorManager()
    {
        Instance = this;
    }

    [MethodRpc((uint)LaunchpadRPC.SyncGradient)]
    public static void RpcSetGradient(PlayerControl pc, int colorId)
    {
        Instance.Gradients[pc.PlayerId] = colorId;
        if (pc.Data is not null)
        {
            pc.SetColor(pc.Data.DefaultOutfit.ColorId);
        }
    }

    [MethodRpc((uint)LaunchpadRPC.RequestGradient)]
    public static void RpcRequestGradient(PlayerControl pc)
    {
        if (!pc.AmOwner) return;
        
        RpcSetGradient(pc,Instance.LocalGradientId);
    }
}