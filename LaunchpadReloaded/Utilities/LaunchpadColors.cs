using UnityEngine;

namespace LaunchpadReloaded.Utilities;
public static class LaunchpadColors
{
    public static CustomColor PureBlack => new (Color.black, Color.black, "Pure Black");
    public static CustomColor PureWhite => new (Color.white, Color.white, "Pure White");
    public static CustomColor HotPink => new (new Color32(238, 0, 108, 255), "Hot Pink");
}