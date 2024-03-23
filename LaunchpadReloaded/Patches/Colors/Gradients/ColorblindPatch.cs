using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Colors.Gradients;

[HarmonyPatch(typeof(CosmeticsLayer))]
public static class ColorblindPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CosmeticsLayer.GetColorBlindText))]
    public static bool CosmeticsLayerPatch(CosmeticsLayer __instance, ref string __result)
    {
        var player = __instance.transform.parent.gameObject.GetComponent<PlayerControl>();
        if (player == null) return true;

        PlayerGradientData comp;

        if (player.TryGetComponent(out comp))
        {
            var defaultColor = Helpers.FirstLetterToUpper(Palette.GetColorName(player.cosmetics.ColorId).ToLower());
            var gradientColor = Helpers.FirstLetterToUpper(Palette.GetColorName(comp.gradientColor).ToLower());

            if (defaultColor == gradientColor || gradientColor == "???")
            {
                __result = defaultColor;
                return false;
            }

            __result = $"{gradientColor}-{defaultColor}";
            return false;
        }
        return true;
    }
}