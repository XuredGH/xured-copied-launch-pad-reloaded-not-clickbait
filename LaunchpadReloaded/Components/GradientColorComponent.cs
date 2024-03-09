using System;
using System.Collections.Generic;
using System.Linq;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class GradientColorComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public Renderer renderer;
    public byte playerId;
    
    public void Start()
    {
        renderer = GetComponent<Renderer>();
        if (!renderer)
        {
            Debug.LogError("No renderer for gradient!");
        }
    }
    
    public void Initialize()
    {
        renderer = GetComponent<Renderer>();
        renderer.material = LaunchpadReloadedPlugin.Mat;
        renderer.sharedMaterial = LaunchpadReloadedPlugin.Mat;
        
        var playerById = GameData.Instance.GetPlayerById(playerId);
        PlayerMaterial.SetColors((playerById != null) ? playerById.DefaultOutfit.ColorId : 0, renderer);
        
        var id = Random.RandomRangeInt(0, 18);
        renderer.sharedMaterial.SetColor(LaunchpadConstants.Body2,Palette.PlayerColors[id]);
        renderer.sharedMaterial.SetColor(LaunchpadConstants.Back2,Palette.ShadowColors[id]);
        renderer.sharedMaterial.SetFloat(LaunchpadConstants.GradStrength, LaunchpadConstants.Strength);
        renderer.sharedMaterial.SetVector(LaunchpadConstants.GradOffset, LaunchpadConstants.Offset);
    }
    
    public void Update()
    {
        var mat2 = renderer.sharedMaterial;

        if (renderer is SpriteRenderer spriteRenderer)
        {
            var rect = spriteRenderer.sprite.rect;
            mat2.SetFloat(LaunchpadConstants.Width,rect.width);
            mat2.SetFloat(LaunchpadConstants.Height,rect.height);
        }
    }
}