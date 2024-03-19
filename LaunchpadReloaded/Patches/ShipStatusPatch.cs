﻿using HarmonyLib;
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
        managers.AddComponent<DragManager>();

        HackingManager.RpcCreateNodes(__instance);
    }


    [HarmonyPrefix, HarmonyPatch(nameof(ShipStatus.SpawnPlayer))]
    public static bool SpawnPlayerPatch(ShipStatus __instance, [HarmonyArgument(2)] bool initialSpawn)
    {
        if (initialSpawn == false && LaunchpadGameOptions.Instance.DisableMeetingTeleport.Value) return false;

        return true;
    }

    /*    [HarmonyPrefix]
        [HarmonyPatch(nameof(ShipStatus.OnDestroy))]
        public static void ShipStatusDestroyPatch(ShipStatus __instance)
        {
            CustomGameModeManager.SetGameMode(0);
            LaunchpadGameOptions.Instance.GameModes.SetValue(0);

            if (__instance.transform.FindChild("LaunchpadManagers")) __instance.transform.FindChild("LaunchpadManagers").gameObject.Destroy();
            //CustomGameModeManager.RpcSetGameMode(PlayerControl.LocalPlayer, 0);
        }*/
}
