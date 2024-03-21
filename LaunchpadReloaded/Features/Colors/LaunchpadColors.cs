using UnityEngine;

namespace LaunchpadReloaded.Features.Colors;
public static class LaunchpadColors
{
    public static CustomColor PureBlack => new(Color.black, Color.black, "Pure Black");
    public static CustomColor PureWhite => new(Color.white, Color.white, "Pure White");
    public static CustomColor HotPink => new(new Color32(238, 0, 108, 255), "Hot Pink");
    public static CustomColor Blueberry => new(new Color32(85, 151, 207, 255), "Blueberry");
    public static CustomColor Mint => new(new Color32(91, 190, 140, 255), "Mint");
    public static CustomColor Lavender => new(new Color32(181, 176, 255, 255), "Lavender");
    public static CustomColor Iris => new(new Color32(90, 79, 207, 255), "Iris");
    public static CustomColor Viridian => new(new Color32(64, 130, 109, 255), "Viridian");
    public static CustomColor Terracotta => new(new Color32(226, 114, 91, 255), "Terracotta");
    public static CustomColor Cherry => new(new Color32(59, 1, 0, 255), "Cherry");
    public static CustomColor Azure => new(new Color32(0, 233, 255, 255), "Azure");
    public static CustomColor Faschia => new(new Color32(192, 32, 64, 255), "Faschia");
    public static CustomColor Mahogany => new(new Color32(173, 109, 104, 255), "Mahogany");
}