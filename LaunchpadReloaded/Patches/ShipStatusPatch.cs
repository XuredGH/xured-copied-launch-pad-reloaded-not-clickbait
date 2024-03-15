using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Patches;
[HarmonyPatch(typeof(ShipStatus))]
public static class ShipStatusPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ShipStatus.Awake))]
    public static void MapLoadingPatch(ShipStatus __instance)
    {
        var managers = new GameObject("LaunchpadManagers");
        managers.transform.SetParent(__instance.transform);
        managers.AddComponent<HackingManager>();
        managers.AddComponent<TrackingManager>();
        managers.AddComponent<ScannerManager>();
        managers.AddComponent<RevivalManager>();

        HackingManager.RpcCreateNodes(__instance);
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(ShipStatus.OnDestroy))]
    public static void ShipStatusDestroyPatch(ShipStatus __instance)
    {
        CustomGamemodeManager.SetGamemode(0);
        LaunchpadGameOptions.Instance.GameModes.SetValue(0);
        //CustomGamemodeManager.RpcSetGamemode(PlayerControl.LocalPlayer, 0);
    }
}
