using Reactor.Localization.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Utilities;
public class CustomColor
{
    public CustomColor(Color mainColor, string name)
    {
        MainColor = mainColor;
        ShadowColor = GetShadowColor(mainColor, 60);
        Name = CustomStringName.CreateAndRegister(name);
    }

    public CustomColor(Color mainColor, Color shadowColor, string name)
    {
        MainColor = mainColor;
        ShadowColor = shadowColor;
        Name = CustomStringName.CreateAndRegister(name);
    }

    public Color MainColor { get; }
    public Color ShadowColor { get; }
    public StringNames Name { get; }

    public Color32 GetShadowColor(Color32 c, byte darknessAmount)
    {
        return
            new((byte)Mathf.Clamp(c.r - darknessAmount, 0, 255), (byte)Mathf.Clamp(c.g - darknessAmount, 0, 255),
            (byte)Mathf.Clamp(c.b - darknessAmount, 0, 255), byte.MaxValue);
    }
}