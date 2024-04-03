using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking;
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

        __instance.RpcCreateNodes();
    }

    /// <summary>
    /// Disable the meeting teleportation if option is enabled
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("SpawnPlayer")]
    public static bool SpawnPlayerPatch([HarmonyArgument(2)] bool initialSpawn)
    {
        return initialSpawn || !LaunchpadGameOptions.Instance.DisableMeetingTeleport.Value;
    }
}
