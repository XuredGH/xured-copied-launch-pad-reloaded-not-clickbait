using System.Collections.Generic;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Features.Managers;

[RegisterInIl2Cpp]
public class DragManager(System.IntPtr ptr) : MonoBehaviour(ptr)
{
    public Dictionary<byte, byte> DraggingPlayers;
    public static DragManager Instance;

    private void Awake()
    {
        Instance = this;
        DraggingPlayers = new Dictionary<byte, byte>();
    }

    private void OnDestroy()
    {
        DraggingPlayers.Clear();
    }

    public bool IsDragging(byte playerId)
    {
        return DraggingPlayers.ContainsKey(playerId);
    }

    [MethodRpc((uint)LaunchpadRPC.StartDrag)]
    public static void RpcStartDragging(PlayerControl playerControl, byte bodyId)
    {
        Instance.DraggingPlayers.Add(playerControl.PlayerId, bodyId);
        playerControl.MyPhysics.Speed /= 2;
        if (playerControl.AmOwner)
        {
            DragButton.Instance.SetDrop();
        }
    }

    [MethodRpc((uint)LaunchpadRPC.StopDrag)]
    public static void RpcStopDragging(PlayerControl playerControl)
    {
        Instance.DraggingPlayers.Remove(playerControl.PlayerId);
        playerControl.MyPhysics.Speed *= 2;
        if (playerControl.AmOwner)
        {
            DragButton.Instance.SetDrag();
        }
    }

}