using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class GradientColorComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public SpriteRenderer renderer;
    public Material mat;

    public Vector4 offset = new(0, .35f, .5f, 1);
    public float strength = 125;

    public int primaryColor;
    public int secondaryColor;
    
    public void SetColor(int color1, int color2)
    {
        renderer = GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            return;
        }

        if (color1 > Palette.PlayerColors.Length || color2 > Palette.PlayerColors.Length)
        {
            return;
        }

        primaryColor = color1;
        secondaryColor = color2;

        mat = renderer.material;

        PlayerMaterial.SetColors(primaryColor, mat);

        mat.SetColor(ShaderID.SecondaryBodyColor, Palette.PlayerColors[secondaryColor]);
        mat.SetColor(ShaderID.SecondaryShadowColor, Palette.ShadowColors[secondaryColor]);
        mat.SetFloat(ShaderID.GradientStrength, strength);
        mat.SetVector(ShaderID.GradientOffset, offset);
    }
    
    public void SetColor(int gradientColor)
    {
        renderer = GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            return;
        }

        if (gradientColor > Palette.PlayerColors.Length)
        {
            return;
        }

        secondaryColor = gradientColor;

        mat = renderer.material;
        
        mat.SetColor(ShaderID.SecondaryBodyColor, Palette.PlayerColors[secondaryColor]);
        mat.SetColor(ShaderID.SecondaryShadowColor, Palette.ShadowColors[secondaryColor]);
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