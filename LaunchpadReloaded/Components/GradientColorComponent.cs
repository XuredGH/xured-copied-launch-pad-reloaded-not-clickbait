using System;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class GradientColorComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public SpriteRenderer renderer;
    public Material mat;

    public Vector4 offset = new (0, .35f, .5f, 1); 
    public float strength = 125;
    
    public void SetColor(int color1 = 0, int color2 = 0)
    {
        renderer = GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            this.Destroy();
            return;
        }

        if (color1 > Palette.PlayerColors.Length || color2 > Palette.PlayerColors.Length)
        {
            Debug.LogError("Invalid color ID!");
            return;
        }
        
        renderer.material = LaunchpadAssets.GradientMaterial.LoadAsset();
        mat = renderer.material;
        
        PlayerMaterial.SetColors(color1, mat);
        
        mat.SetColor(ShaderID.SecondaryBodyColor,Palette.PlayerColors[color2]);
        mat.SetColor(ShaderID.SecondaryShadowColor,Palette.ShadowColors[color2]);
        mat.SetFloat(ShaderID.GradientStrength, strength);
        mat.SetVector(ShaderID.GradientOffset, offset);
        
    }

    public void Update()
    {
        if (!renderer || !renderer.sprite)
        {
            return;
        }

        var rect = renderer.sprite.rect;
        mat.SetFloat(ShaderID.SpriteWidth, rect.width);
        mat.SetFloat(ShaderID.SpriteHeight, rect.height);

    }
}