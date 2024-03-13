using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Extensions;
using System.Linq;
using UnityEngine;

namespace LaunchpadReloaded.Patches;
[HarmonyPatch(typeof(ShipStatus))]
public static class ShipStatusPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ShipStatus.Awake))]
    public static void MapLoadingPatch(ShipStatus __instance)
    {
        GameObject managers = new GameObject("LaunchpadManagers");
        managers.transform.SetParent(__instance.transform);
        managers.AddComponent<HackingManager>();
        managers.AddComponent<TrackingManager>();
        managers.AddComponent<ScannerManager>();

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
