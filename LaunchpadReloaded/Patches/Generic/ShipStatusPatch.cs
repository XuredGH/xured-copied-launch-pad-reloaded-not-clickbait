using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(ShipStatus))]
public static class ShipStatusPatch
{
    /// <summary>
    /// Add all the managers for the game (probably not the best or cleanest way to do it, but it works).
    /// Will be replaced with <see cref="LaunchpadPlayer"/> eventually
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("Awake")]
    public static void MapLoadingPatch(ShipStatus __instance)
    {
        var managers = new GameObject("LaunchpadManagers");
        managers.transform.SetParent(__instance.transform);
        managers.AddComponent<HackingManager>();

        foreach (var player in LaunchpadPlayer.GetAllPlayers())
        {
            player.ResetPlayer();
        }

        __instance.RpcCreateNodes();
    }

    /// <summary>
    /// Disable the meeting teleportation if option is enabled
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("SpawnPlayer")]
    public static bool SpawnPlayerPatch([HarmonyArgument(2)] bool initialSpawn)
    {
        if (TutorialManager.InstanceExists) return true;
        return initialSpawn || !ModdedGroupSingleton<GeneralOptions>.Instance.DisableMeetingTeleport;
    }
}
