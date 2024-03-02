using System.Collections.Generic;
using System.Linq;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Features;

public static class DragManager
{
    public static readonly Dictionary<byte, byte> DraggingPlayers = new();

    public static bool IsDragging(byte playerId)
    {
        return DraggingPlayers.ContainsKey(playerId);
    }

}