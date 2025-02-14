using Reactor.Utilities.Attributes;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class ReapedBodyComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public DeadBody? Body;

    private void Awake()
    {
        Body = GetComponent<DeadBody>();
    }
}