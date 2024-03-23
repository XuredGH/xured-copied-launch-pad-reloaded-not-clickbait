using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities;

namespace LaunchpadReloaded.Patches.Colors.Gradients;

[HarmonyPatch(typeof(CosmeticsLayer))]
public static class ColorblindPatch
{
    [HarmonyPrefix, HarmonyPatch(nameof(CosmeticsLayer.GetColorBlindText))]
    public static bool CosmeticsLayerPatch(CosmeticsLayer __instance, ref string __result)
    {
        if (!__instance.TryGetComponent(out PlayerGradientData comp) &&
            !__instance.transform.parent.TryGetComponent(out comp))
        {
            Logger<LaunchpadReloadedPlugin>.Error(__instance.transform.parent.name);
            return true;
        }

        if (!comp.GradientEnabled)
        {
            return true;
        }
        
        var defaultColor = Helpers.FirstLetterToUpper(Palette.GetColorName(__instance.ColorId).ToLower());
        var gradientColor = Helpers.FirstLetterToUpper(Palette.GetColorName(comp.GradientColor).ToLower());

        if (defaultColor == gradientColor || gradientColor == "???")
        {
            __result = defaultColor;
            return false;
        }

        __result = $"{gradientColor}-{defaultColor}";
        return false;
    }
}