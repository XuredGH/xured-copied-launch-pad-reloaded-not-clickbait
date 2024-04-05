using System;
using LaunchpadReloaded.Features.Managers;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class PlayerGradientData(IntPtr ptr) : MonoBehaviour(ptr)
{
    public byte playerId;

    public PlayerControl player;
    
    private int _gradientColor = Random.RandomRangeInt(0, Palette.PlayerColors.Length);
    private bool _gradientEnabled = true;

    public bool GradientEnabled
    {
        get
        {
            if (player)
            {
                return _gradientEnabled;
            }

            if (GradientManager.TryGetEnabled(playerId, out var gradEnabled))
            {
                return gradEnabled;
            }

            Logger<LaunchpadReloadedPlugin>.Error("No gradient data found!");
            return true;
        }
        set => _gradientEnabled = value;
    }
    
    public int GradientColor
    {
        get
        {
            if (player)
            {
                return _gradientColor;
            }

            if (GradientManager.TryGetColor(playerId, out var color))
            {
                return color;
            }

            Logger<LaunchpadReloadedPlugin>.Error("No gradient color found!");
            return _gradientColor;
        }
        set => _gradientColor = value;
    }

    public void Awake()
    {
        player = GetComponent<PlayerControl>();
        if (!player)
        {
            return;
        }
        
        playerId = player.PlayerId;

        if (player.AmOwner && AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
        {
            _gradientColor = GradientManager.LocalGradientId;
        }
    }
}