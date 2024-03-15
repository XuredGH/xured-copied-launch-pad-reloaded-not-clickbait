using System;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class PlayerGradientData(IntPtr ptr) : MonoBehaviour(ptr)
{
    public int gradientColor = 255;
    
    
}