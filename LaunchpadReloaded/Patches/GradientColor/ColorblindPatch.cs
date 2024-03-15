using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(CosmeticsLayer))]
public static class ColorblindPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CosmeticsLayer.GetColorBlindText))]
    public static bool CosmeticsLayerPatch(CosmeticsLayer __instance, ref string __result)
    {
        PlayerControl player = __instance.transform.parent.gameObject.GetComponent<PlayerControl>();
        if (player == null) return true;

        PlayerGradientData comp;

        if (player.TryGetComponent<PlayerGradientData>(out comp))
        {
            string defaultColor = Helpers.FirstLetterToUpper(Palette.GetColorName(player.cosmetics.ColorId).ToLower());
            string gradientColor = Helpers.FirstLetterToUpper(Palette.GetColorName(comp.gradientColor).ToLower());

            if (defaultColor == gradientColor || gradientColor == "???")
            {
                __result = defaultColor;
                return false;
            }

            __result = $"{defaultColor}-{gradientColor}";
            return false;
        }
        return true;
    }
}