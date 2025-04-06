using MiraAPI.Colors;
using MiraAPI.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Features.Colors;

[RegisterCustomColors]
public static class LaunchpadColors
{
    public static CustomColor PureBlack { get; } = new CustomColor("Pure Black", Color.black, Color.black);
    public static CustomColor PureWhite { get; } = new CustomColor("Pure White", Color.white, Color.white.DarkenColor(.05f));
    public static CustomColor HotPink { get; } = new CustomColor("Hot Pink", new Color32(238, 0, 108, 255));
    public static CustomColor Blueberry { get; } = new CustomColor("Blueberry", new Color32(85, 151, 207, 255));
    public static CustomColor Mint { get; } = new CustomColor("Mint", new Color32(91, 190, 140, 255));
    public static CustomColor Lavender { get; } = new CustomColor("Lavender", new Color32(181, 176, 255, 255));
    public static CustomColor Iris { get; } = new CustomColor("Iris", new Color32(90, 79, 207, 255));
    public static CustomColor Viridian { get; } = new CustomColor("Viridian", new Color32(64, 130, 109, 255));
    public static CustomColor Blurple { get; } = new CustomColor("Blurple", new Color32(114, 137, 218, 255), new Color32(80, 96, 153, 255));
}