using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using System.Collections.Generic;

namespace LaunchpadReloaded.Features;

public static class DragManager
{
    public static readonly Dictionary<byte, byte> DraggingPlayers = new();

    public static bool IsDragging(byte playerId)
    {
        return DraggingPlayers.ContainsKey(playerId);
    }

    [MethodRpc((uint)LaunchpadRPC.StartDrag)]
    public static void RpcStartDragging(PlayerControl playerControl, byte bodyId)
    {
        DraggingPlayers.Add(playerControl.PlayerId, bodyId);
        playerControl.MyPhysics.Speed /= 2;
        if (playerControl.AmOwner)
        {
            DragButton.Instance.SetDrop();
        }
    }

    [MethodRpc((uint)LaunchpadRPC.StopDrag)]
    public static void RpcStopDragging(PlayerControl playerControl)
    {
        DraggingPlayers.Remove(playerControl.PlayerId);
        playerControl.MyPhysics.Speed *= 2;
        if (playerControl.AmOwner)
        {
            DragButton.Instance.SetDrag();
        }
    }

}