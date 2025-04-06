using MiraAPI.Colors;
using MiraAPI.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Features.Colors;

[RegisterCustomColors]
public static class LaunchpadColors
{
    public static CustomColor PureBlack { get; } = new("Pure Black", Color.black);
    public static CustomColor PureWhite { get; } = new("Pure White", Color.white, Color.white.DarkenColor(0.05f));
    public static CustomColor LightGray { get; } = new("Light Gray", new Color32(211, 211, 211, 255));
    public static CustomColor DarkGray { get; } = new("Dark Gray", new Color32(64, 64, 64, 255));
    public static CustomColor OldLace { get; } = new("Old Lace", new Color32(255, 242, 217, 255));
    public static CustomColor Void { get; } = new("Void", new Color32(0, 0, 0, 255));
    public static CustomColor Shiny { get; } = new("Shiny", new Color32(0, 0, 0, 255));
    public static CustomColor Blood { get; } = new("Blood", new Color32(171, 0, 23, 255));
    public static CustomColor Burgundy { get; } = new("Burgundy", new Color32(128, 0, 32, 255));
    public static CustomColor Crimson { get; } = new("Crimson", new Color32(220, 20, 60, 255));
    public static CustomColor LightRed { get; } = new("Light Red", new Color32(255, 102, 102, 255));
    public static CustomColor DarkRed { get; } = new("Dark Red", new Color32(139, 0, 0, 255));
    public static CustomColor PastelRed { get; } = new("Pastel Red", new Color32(255, 153, 153, 255));
    public static CustomColor Amber { get; } = new("Amber", new Color32(255, 191, 0, 255));
    public static CustomColor Apricot { get; } = new("Apricot", new Color32(251, 206, 177, 255));
    public static CustomColor Coral { get; } = new("Coral", new Color32(255, 127, 80, 255));
    public static CustomColor Peach { get; } = new("Peach", new Color32(255, 203, 164, 255));
    public static CustomColor PastelOrange { get; } = new("Pastel Orange", new Color32(255, 179, 128, 255));
    public static CustomColor LightOrange { get; } = new("Light Orange", new Color32(255, 160, 122, 255));
    public static CustomColor DarkOrange { get; } = new("Dark Orange", new Color32(255, 140, 0, 255));
    public static CustomColor Gold { get; } = new("Gold", new Color32(255, 214, 0, 255));
    public static CustomColor Lemon { get; } = new("Lemon", new Color32(253, 245, 191, 255));
    public static CustomColor Sunflower { get; } = new("Sunflower", new Color32(255, 218, 89, 255));
    public static CustomColor DarkBanana { get; } = new("Dark Banana", new Color32(189, 184, 107, 255));
    public static CustomColor Emerald { get; } = new("Emerald", new Color32(80, 200, 120, 255));
    public static CustomColor ForestGreen { get; } = new("Forest Green", new Color32(34, 139, 34, 255));
    public static CustomColor Jade { get; } = new("Jade", new Color32(0, 168, 107, 255));
    public static CustomColor Mint { get; } = new("Mint", new Color32(91, 190, 140, 255));
    public static CustomColor PastelGreen { get; } = new("Pastel Green", new Color32(119, 221, 119, 255));
    public static CustomColor Seafoam { get; } = new("Seafoam", new Color32(159, 226, 191, 255));
    public static CustomColor Viridian { get; } = new("Viridian", new Color32(64, 130, 109, 255));
    public static CustomColor PastelMint { get; } = new("Pastel Mint", new Color32(189, 252, 201, 255));
    public static CustomColor Azure { get; } = new("Azure", new Color32(0, 127, 255, 255));
    public static CustomColor Blueberry { get; } = new("Blueberry", new Color32(85, 151, 207, 255));
    public static CustomColor Blurple { get; } = new("Blurple", new Color32(114, 137, 218, 255), new Color32(80, 96, 153, 255));
    public static CustomColor Cerulean { get; } = new("Cerulean", new Color32(0, 122, 166, 255));
    public static CustomColor Cobalt { get; } = new("Cobalt", new Color32(0, 71, 171, 255));
    public static CustomColor Navy { get; } = new("Navy", new Color32(0, 0, 128, 255));
    public static CustomColor PastelBlue { get; } = new("Pastel Blue", new Color32(174, 198, 207, 255));
    public static CustomColor SkyBlue { get; } = new("Sky Blue", new Color32(135, 206, 235, 255));
    public static CustomColor Teal { get; } = new("Teal", new Color32(0, 128, 128, 255));
    public static CustomColor Amethyst { get; } = new("Amethyst", new Color32(153, 102, 204, 255));
    public static CustomColor Blackcurrant { get; } = new("Blackcurrant", new Color32(17, 0, 28, 255));
    public static CustomColor DarkPurple { get; } = new("Dark Currant", new Color32(41, 0, 37, 255));
    public static CustomColor DirtyPurple { get; } = new("Dirty Purple", new Color32(99, 59, 89, 255));
    public static CustomColor Iris { get; } = new("Iris", new Color32(90, 79, 207, 255));
    public static CustomColor Lavender { get; } = new("Lavender", new Color32(181, 176, 255, 255));
    public static CustomColor Orchid { get; } = new("Orchid", new Color32(199, 113, 150, 255));
    public static CustomColor PastelLilac { get; } = new("Pastel Lilac", new Color32(221, 160, 221, 255));
    public static CustomColor PastelPurple { get; } = new("Pastel Purple", new Color32(179, 158, 181, 255));
    public static CustomColor Violet { get; } = new("Violet", new Color32(143, 0, 255, 255));
    public static CustomColor CottonCandy { get; } = new("Cotton Candy", new Color32(255, 189, 222, 255));
    public static CustomColor HotPink { get; } = new("Hot Pink", new Color32(238, 0, 108, 255));
    public static CustomColor PastelPink { get; } = new("Pastel Pink", new Color32(255, 209, 220, 255));
    public static CustomColor Salmon { get; } = new("Salmon", new Color32(250, 128, 114, 255));
    public static CustomColor DeepTaupe { get; } = new("Deep Taupe", new Color32(126, 94, 96, 255));
    public static CustomColor Mahogany { get; } = new("Mahogany", new Color32(192, 64, 0, 255));
    public static CustomColor Sepia { get; } = new("Sepia", new Color32(112, 66, 20, 255));
    public static CustomColor Taupe { get; } = new("Taupe", new Color32(72, 60, 50, 255));
}
