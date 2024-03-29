using HarmonyLib;
using LaunchpadReloaded.API.Settings;

namespace LaunchpadReloaded.Patches.Settings;

[HarmonyPatch(typeof(OptionsMenuBehaviour))]
public static class OptionsMenuPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(OptionsMenuBehaviour.Start))]
    public static void StartPostfix(OptionsMenuBehaviour __instance)
    {
        CustomSettingsManager.CreateTab(__instance);
    }
}