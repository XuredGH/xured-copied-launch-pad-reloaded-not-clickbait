using MiraAPI.Utilities.Colors;
using UnityEngine;

namespace LaunchpadReloaded.Features.Colors;


[RegisterCustomColors]
public static class LaunchpadColors
{
    public static CustomColor PureBlack { get; } = new(Color.black, Color.black, "Pure Black");
    public static CustomColor PureWhite { get; } = new(Color.white, Color.white, "Pure White");
    public static CustomColor HotPink { get; } = new(new Color32(238, 0, 108, 255), "Hot Pink");
    public static CustomColor Blueberry { get; } = new(new Color32(85, 151, 207, 255), "Blueberry");
    public static CustomColor Mint { get; } = new(new Color32(91, 190, 140, 255), "Mint");
    public static CustomColor Lavender { get; } = new(new Color32(181, 176, 255, 255), "Lavender");
    public static CustomColor Iris { get; } = new(new Color32(90, 79, 207, 255), "Iris");
    public static CustomColor Viridian { get; } = new(new Color32(64, 130, 109, 255), "Viridian");
    public static CustomColor Blurple { get; } = new(new Color32(114, 137, 218, 255), new Color32(80, 96, 153, 255), "Blurple");
}