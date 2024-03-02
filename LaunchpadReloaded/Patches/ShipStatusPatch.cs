using HarmonyLib;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches;
[HarmonyPatch(typeof(ShipStatus))]
public static class ShipStatusPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ShipStatus.Begin))]
    public static void MapLoadingPatch(ShipStatus __instance)
    {
        HackingManager.RpcCreateNodes(__instance);
    }
}
