using System.Collections.Generic;
using System.Linq;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Features;

public static class DragManager
{
    public static readonly Dictionary<byte, byte> DraggingPlayers = new();
    
    [MethodRpc((uint)LaunchpadRPC.StartDrag)]
    public static void RpcStartDragging(PlayerControl playerControl, byte bodyId)
    {
        DraggingPlayers.Add(playerControl.PlayerId, bodyId);
        playerControl.MyPhysics.Speed /= 2;
    }
    
    [MethodRpc((uint)LaunchpadRPC.StopDrag)]
    public static void RpcStopDragging(PlayerControl playerControl)
    {
        DraggingPlayers.Remove(playerControl.PlayerId);
        playerControl.MyPhysics.Speed *= 2;
    }
    
    public static DeadBody GetBodyById(byte id)
    {
        return Object.FindObjectsOfType<DeadBody>().FirstOrDefault(body => body.ParentId == id);
    }

}