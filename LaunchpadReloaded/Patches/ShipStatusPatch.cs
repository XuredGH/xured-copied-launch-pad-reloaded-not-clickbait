using HarmonyLib;
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
}
