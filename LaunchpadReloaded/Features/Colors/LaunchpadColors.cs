using MiraAPI.Colors;
using MiraAPI.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Features.Colors;

[RegisterCustomColors]
public static class LaunchpadColors
{
    public static CustomColor PureBlack { get; } = new("Pure Black", Color.black, Color.black);
    public static CustomColor PureWhite { get; } = new("Pure White", Color.white, Color.white.DarkenColor(.05f));
    public static CustomColor HotPink { get; } = new("Hot Pink", new Color32(238, 0, 108, 255));
    public static CustomColor Blueberry { get; } = new("Blueberry", new Color32(85, 151, 207, 255));
    public static CustomColor Mint { get; } = new("Mint", new Color32(91, 190, 140, 255));
    public static CustomColor Lavender { get; } = new("Lavender", new Color32(181, 176, 255, 255));
    public static CustomColor Iris { get; } = new("Iris", new Color32(90, 79, 207, 255));
    public static CustomColor Viridian { get; } = new("Viridian", new Color32(64, 130, 109, 255));
    public static CustomColor Blurple { get; } = new("Blurple", new Color32(114, 137, 218, 255), new Color32(80, 96, 153, 255));
    public static CustomColor Cerulean { get; } = new("Cerulean", new Color32(0, 122, 166, 255));
    public static CustomColor Gold { get; } = new("Gold", new Color32(255, 214, 0, 255));
    public static CustomColor Void { get; } = new("Void", new Color32(0, 0, 0, 255));
    public static CustomColor DeepTaupe { get; } = new("Deep Taupe", new Color32(126, 94, 96, 255));
    public static CustomColor Wine { get; } = new("Wine", new Color32(103, 49, 71, 255));
    public static CustomColor DarkSlateGray { get; } = new("Dark Slate Gray", new Color32(47, 79, 79, 255));
    public static CustomColor Amethyst { get; } = new("Amethyst", new Color32(153, 102, 204, 255));
    public static CustomColor ForestGreen { get; } = new("Forest Green", new Color32(34, 139, 34, 255));
    public static CustomColor OldLace { get; } = new("Old Lace", new Color32(255, 242, 217, 255));
    public static CustomColor DarkBanana { get; } = new("Dark Banana", new Color32(189, 184, 107, 255));
    public static CustomColor Orchid { get; } = new("Orchid", new Color32(199, 113, 150, 255));
    public static CustomColor Lemon { get; } = new("Lemon", new Color32(253, 245, 191, 255));
    public static CustomColor DarkPurple { get; } = new("Dark Currant", new Color32(41, 0, 37, 255));
    public static CustomColor Blackcurrant { get; } = new("Blackcurrant", new Color32(17, 0, 28, 255));
    public static CustomColor Shiny { get; } = new("Shiny", new Color32(0, 0, 0, 255));
    public static CustomColor CottonCandy { get; } = new("Cotton Candy", new Color32(255, 189, 222, 255));
    public static CustomColor Blood { get; } = new("Blood", new Color32(171, 0, 23, 255));
    public static CustomColor DirtyPink { get; } = new("Dirty Pink", new Color32(99, 59, 89, 255));
}