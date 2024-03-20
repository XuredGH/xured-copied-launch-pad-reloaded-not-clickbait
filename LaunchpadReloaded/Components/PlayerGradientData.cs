using Reactor.Utilities.Attributes;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class PlayerGradientData(IntPtr ptr) : MonoBehaviour(ptr)
{
    private int _gradientColor = Random.RandomRangeInt(0, Palette.PlayerColors.Length);
    
    public int GradientColor
    {
        get
        {
            if (!transform.parent)
            {
                return _gradientColor;
            }
            
            if (transform.parent.TryGetComponent<PlayerGradientData>(out var gradientData))
            {
                return _gradientColor = gradientData.GradientColor;
            }

            return _gradientColor;
        }
        set => _gradientColor = value;
    }
}