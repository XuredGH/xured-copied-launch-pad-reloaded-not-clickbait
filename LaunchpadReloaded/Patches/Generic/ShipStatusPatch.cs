using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(ShipStatus))]
public static class ShipStatusPatch
{
    /// <summary>
    /// Add all the managers for the game (probably not the best or cleanest way to do it, but it works)
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("Awake")]
    public static void MapLoadingPatch(ShipStatus __instance)
    {
        var managers = new GameObject("LaunchpadManagers");
        managers.transform.SetParent(__instance.transform);
        managers.AddComponent<HackingManager>();
        managers.AddComponent<TrackingManager>();
        managers.AddComponent<ScannerManager>();
        managers.AddComponent<RevivalManager>();
        managers.AddComponent<DragManager>();

        HackingManager.RpcCreateNodes(__instance);
    }

    /// <summary>
    /// Disable the meeting teleportation if option is enabled
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("SpawnPlayer")]
    public static bool SpawnPlayerPatch([HarmonyArgument(2)] bool initialSpawn)
    {
        if (initialSpawn == false && LaunchpadGameOptions.Instance.DisableMeetingTeleport.Value) return false;
        return true;
    }
}
