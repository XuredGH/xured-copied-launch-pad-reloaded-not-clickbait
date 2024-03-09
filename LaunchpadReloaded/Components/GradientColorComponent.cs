using System;
using System.Collections.Generic;
using System.Linq;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class GradientColorComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public SpriteRenderer renderer;
    public Material mat;

    public readonly int ShaderWidth = Shader.PropertyToID("_Width");
    public readonly int ShaderHeight = Shader.PropertyToID("_Height");
    public readonly int Body2 = Shader.PropertyToID("_BodyColor2");
    public readonly int Back2 = Shader.PropertyToID("_BackColor2");
    public readonly int GradOffset = Shader.PropertyToID("_GradientOffset");
    public readonly int GradStrength = Shader.PropertyToID("_GradientStrength");

    public Vector4 offset = new (0, .35f, .5f, 1); 
    public float strength = 125;
    
    public void SetColor(int color1, int color2 = 0)
    {
        renderer = GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            this.Destroy();
            return;
        }
        
        renderer.material = LaunchpadReloadedPlugin.Mat;
        mat = renderer.material;
        
        PlayerMaterial.SetColors(color1, mat);
        
        mat.SetColor(Body2,Palette.PlayerColors[color2]);
        mat.SetColor(Back2,Palette.ShadowColors[color2]);
        mat.SetFloat(GradStrength, strength);
        mat.SetVector(GradOffset, offset);
        
    }

    public void Update()
    {
        if (renderer && renderer.sprite)
        {
            var rect = renderer.sprite.rect;
            mat.SetFloat(ShaderWidth, rect.width);
            mat.SetFloat(ShaderHeight, rect.height);
        }
        
    }
}