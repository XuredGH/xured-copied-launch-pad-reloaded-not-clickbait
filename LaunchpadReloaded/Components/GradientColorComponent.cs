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

    private readonly int _shaderWidth = Shader.PropertyToID("_Width");
    private readonly int _shaderHeight = Shader.PropertyToID("_Height");
    private readonly int _body2 = Shader.PropertyToID("_BodyColor2");
    private readonly int _back2 = Shader.PropertyToID("_BackColor2");
    private readonly int _gradOffset = Shader.PropertyToID("_GradientOffset");
    private readonly int _gradStrength = Shader.PropertyToID("_GradientStrength");

    public void SetColor(int color1 = 0, int color2 = 0)
    {
        renderer = GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            this.Destroy();
            return;
        }
        
        renderer.material = LaunchpadAssets.GradientMaterial.LoadAsset();
        mat = renderer.material;
        
        PlayerMaterial.SetColors(color1, mat);
        
        mat.SetColor(_body2,Palette.PlayerColors[color2]);
        mat.SetColor(_back2,Palette.ShadowColors[color2]);
        mat.SetFloat(_gradStrength, strength);
        mat.SetVector(_gradOffset, offset);
        
    }

    public void Update()
    {
        if (!renderer || !renderer.sprite)
        {
            return;
        }

        var rect = renderer.sprite.rect;
        mat.SetFloat(_shaderWidth, rect.width);
        mat.SetFloat(_shaderHeight, rect.height);

    }
}