using System;
using System.Collections.Generic;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Features.Managers;

[RegisterInIl2Cpp]
public class DragManager(IntPtr ptr) : MonoBehaviour(ptr)
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

    [MethodRpc((uint)LaunchpadRpc.StartDrag)]
    public static void RpcStartDragging(PlayerControl playerControl, byte bodyId)
    {
        if (playerControl.Data.Role is not JanitorRole)
        {
            return;
        }
        
        Instance.DraggingPlayers.Add(playerControl.PlayerId, bodyId);
        playerControl.MyPhysics.Speed = GameOptionsManager.Instance.currentNormalGameOptions.PlayerSpeedMod/2;
        if (playerControl.AmOwner)
        {
            DragButton.Instance.SetDrop();
        }
    }

    [MethodRpc((uint)LaunchpadRpc.StopDrag)]
    public static void RpcStopDragging(PlayerControl playerControl)
    {
        Instance.DraggingPlayers.Remove(playerControl.PlayerId);
        playerControl.MyPhysics.Speed = GameOptionsManager.Instance.currentNormalGameOptions.PlayerSpeedMod;
        if (playerControl.AmOwner)
        {
            DragButton.Instance.SetDrag();
        }
    }

}