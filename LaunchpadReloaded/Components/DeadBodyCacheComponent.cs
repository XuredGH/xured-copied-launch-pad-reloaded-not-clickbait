using System;
using System.Collections.Generic;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Components;

// this exists for performance purposes.
// it is a lot quicker to loop over the AllBodies list than to do an Object.FindObjectsOfType every frame (or fixed update)
[RegisterInIl2Cpp]
public class DeadBodyCacheComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static readonly List<DeadBodyCacheComponent> AllBodies = [];

    public DeadBody body;

    public bool hidden;
    
    public void SetVisibility(bool visible)
    {
        hidden = !visible;
        body.enabled = visible;
        foreach (var spriteRenderer in body.bodyRenderers)
        {
            spriteRenderer.enabled = visible;
        }
    }

private void Awake()
    {
        if (!TryGetComponent(out body))
        {
            Logger<LaunchpadReloadedPlugin>.Error("No dead body found for component! Destroying!");
            this.Destroy();
            return;
        }
        Debug.LogError(body.ParentId);
        
        AllBodies.Add(this);
    }

    private void OnDestroy()
    {
        AllBodies.Remove(this);
    }
}