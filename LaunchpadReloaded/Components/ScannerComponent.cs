using LaunchpadReloaded.Features;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class ScannerComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public PlayerControl PlacedBy;
    public byte Id;
    public List<PlayerControl> PlayersInProximity = new List<PlayerControl>();

    public void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.gameObject.GetComponent<PlayerControl>();
        if (player == null) return;

        if(!PlayersInProximity.Contains(player)) PlayersInProximity.Add(player);
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        var player = collider.gameObject.GetComponent<PlayerControl>();
        if (player == null) return;

        if (PlayersInProximity.Contains(player)) PlayersInProximity.Remove(player);
    }
}
