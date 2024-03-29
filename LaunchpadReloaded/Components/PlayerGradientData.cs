using System;
using LaunchpadReloaded.Features.Managers;
using Reactor.Utilities.Attributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class PlayerGradientData(IntPtr ptr) : MonoBehaviour(ptr)
{
    private int _gradientColor = Random.RandomRangeInt(0, Palette.PlayerColors.Length);

    public byte playerId;

    private bool _gradientEnabled = true;

    public bool GradientEnabled
    {
        get
        {
            if (GetComponent<PlayerControl>())
            {
                return _gradientEnabled;
            }

            if (GradientManager.TryGetEnabled(playerId, out var gradEnabled))
            {
                return gradEnabled;
            }

            throw new InvalidOperationException("No gradient data found!");
        }
        set => _gradientEnabled = value;
    }
    
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