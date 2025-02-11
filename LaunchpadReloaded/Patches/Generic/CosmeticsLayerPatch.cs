using HarmonyLib;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(CosmeticsLayer))]
public static class CosmeticsLayerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CosmeticsLayer), nameof(CosmeticsLayer.ToggleNameVisible))]
    public static void ToggleNamePatch(CosmeticsLayer __instance)
    {
        // fix
    }
}