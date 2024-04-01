using System;
using System.Collections.Generic;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Features.Managers;

[RegisterInIl2Cpp]
public class RevivalManager(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static RevivalManager Instance;
    public List<byte> revivedPlayers;

    private void Awake()
    {
        Instance = this;
        revivedPlayers = new List<byte>();
    }

    private void OnDestroy()
    {
        revivedPlayers.Clear();
    }
}