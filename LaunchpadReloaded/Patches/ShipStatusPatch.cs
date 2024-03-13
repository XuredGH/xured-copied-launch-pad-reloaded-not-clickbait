using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.Components;
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

    [HarmonyPrefix]
    [HarmonyPatch(nameof(ShipStatus.OnDestroy))]
    public static void ShipStatusDestroyPatch(ShipStatus __instance)
    {
        CustomGamemodeManager.SetGamemode(0);
        LaunchpadGameOptions.Instance.Gamemodes.SetValue(0); 
        //CustomGamemodeManager.RpcSetGamemode(PlayerControl.LocalPlayer, 0);
    }
}
