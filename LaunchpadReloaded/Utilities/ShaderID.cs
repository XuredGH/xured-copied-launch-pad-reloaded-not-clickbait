using UnityEngine;

namespace LaunchpadReloaded.Utilities;

public static class ShaderID
{
    public static readonly int GradientOffset = Shader.PropertyToID("_GradientOffset");
    public static readonly int GradientStrength = Shader.PropertyToID("_GradientStrength");
    public static readonly int SecondaryBodyColor = Shader.PropertyToID("_BodyColor2");
    public static readonly int SecondaryShadowColor = Shader.PropertyToID("_BackColor2");
    public static readonly int SpriteHeight = Shader.PropertyToID("_Height");
    public static readonly int SpriteWidth = Shader.PropertyToID("_Width");
    public static readonly int Outline = Shader.PropertyToID("_Outline");
    public static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
    public static readonly int AddColor = Shader.PropertyToID("_AddColor");
}