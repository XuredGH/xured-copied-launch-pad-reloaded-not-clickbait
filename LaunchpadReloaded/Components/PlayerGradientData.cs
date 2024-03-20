using Reactor.Utilities.Attributes;
using System;
using LaunchpadReloaded.Features;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class PlayerGradientData(IntPtr ptr) : MonoBehaviour(ptr)
{
    private int _gradientColor = Random.RandomRangeInt(0, Palette.PlayerColors.Length);

    public byte playerId;
    
    public int GradientColor
    {
        get
        {
            if (GetComponent<PlayerControl>())
            {
                return _gradientColor;
            }

            if (GradientManager.TryGetColor(playerId, out var color))
            {
                return color;
            }

            throw new InvalidOperationException("No gradient color found!");
        }
        set => _gradientColor = value;
    }
}