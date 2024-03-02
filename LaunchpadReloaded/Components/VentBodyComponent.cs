using System;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class VentBodyComponent (IntPtr ptr) : MonoBehaviour (ptr)
{
    public DeadBody deadBody;

    public void ExposeBody()
    {
        deadBody.ShowBody(false);
    }
    
}