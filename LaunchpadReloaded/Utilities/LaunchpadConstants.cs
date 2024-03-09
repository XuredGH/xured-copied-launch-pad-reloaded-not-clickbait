using UnityEngine;

namespace LaunchpadReloaded.Utilities;

public class LaunchpadConstants
{
    
    // shader properties
    public static readonly int Width = Shader.PropertyToID("_Width");
    public static readonly int Height = Shader.PropertyToID("_Height");
    public static readonly int Body2 = Shader.PropertyToID("_BodyColor2");
    public static readonly int Back2 = Shader.PropertyToID("_BackColor2");
    public static readonly int GradOffset = Shader.PropertyToID("_GradientOffset");
    public static readonly int GradStrength = Shader.PropertyToID("_GradientStrength");

    public static readonly Vector4 Offset = new (0, .275f, .4f, 1);
    public static readonly float Strength = 125;
}