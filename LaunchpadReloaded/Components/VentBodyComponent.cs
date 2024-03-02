using System;
using LaunchpadReloaded.API.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;

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