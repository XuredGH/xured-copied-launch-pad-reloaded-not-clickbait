using Reactor.Localization.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Utilities;
public class CustomColor
{
    public CustomColor(Color32 mainColor, string name)
    {
        MainColor = mainColor;
        ShadowColor = GetShadowColor(mainColor, 60);
        Name = CustomStringName.CreateAndRegister(name);
    }

    public CustomColor(Color32 mainColor, Color32 shadowColor, string name)
    {
        MainColor = mainColor;
        ShadowColor = shadowColor;
        Name = CustomStringName.CreateAndRegister(name);
    }

    public Color32 MainColor { get; }
    public Color32 ShadowColor { get; }
    public StringNames Name { get; }

    public static Color32 GetShadowColor(Color32 c, byte darknessAmount)
    {
        return
            new Color32((byte)Mathf.Clamp(c.r - darknessAmount, 0, 255), (byte)Mathf.Clamp(c.g - darknessAmount, 0, 255),
            (byte)Mathf.Clamp(c.b - darknessAmount, 0, 255), byte.MaxValue);
    }
}