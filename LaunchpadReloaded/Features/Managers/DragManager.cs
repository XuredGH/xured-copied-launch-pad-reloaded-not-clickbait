using System;
using System.Collections.Generic;
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
}