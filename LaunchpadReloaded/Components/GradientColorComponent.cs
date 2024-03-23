using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System;
using Reactor.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class GradientColorComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public SpriteRenderer renderer;
    public Material mat;
    
    public int primaryColor;
    public int secondaryColor;
    
    public void SetColor(int color1, int color2)
    {
        renderer = GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            return;
        }

        if (color2 > Palette.PlayerColors.Length || color2<0)
        {
            if (color1 >= 0 && color1 < Palette.PlayerColors.Length)
            {
                PlayerMaterial.SetColors(primaryColor, mat);
            }
            return;
        }
        
        primaryColor = color1;
        secondaryColor = color2;

        mat = renderer.material;

        PlayerMaterial.SetColors(primaryColor, mat);

        mat.SetColor(ShaderID.SecondaryBodyColor, Palette.PlayerColors[secondaryColor]);
        mat.SetColor(ShaderID.SecondaryShadowColor, Palette.ShadowColors[secondaryColor]);
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
    }

    public void Update()
    {
        if (!renderer)
        {
            return;
        }

        mat.SetInt(ShaderID.Flip, renderer.flipX ? 1 : 0);

    }
}