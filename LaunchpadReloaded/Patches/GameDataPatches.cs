using System;
using System.Linq;
using HarmonyLib;
using LaunchpadReloaded.Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(GameData))]
public static class GameDataPatches
{
    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    public static void AwakePostfix(GameData __instance)
    {
        __instance.gameObject.AddComponent<CustomGameData>();
    }
    

    [HarmonyPostfix]
    [HarmonyPatch("AddPlayer")]
    public static void AddPlayerPostfix(GameData __instance, ref GameData.PlayerInfo __result)
    {
        __instance.GetComponent<CustomGameData>().CustomPlayerInfos.Add(
            __result.PlayerId,
            new CustomGameData.CustomPlayerInfo(
                __result.PlayerId,Random.RandomRangeInt(0,Palette.PlayerColors.Length))
        );
        
    }
    
    [HarmonyPostfix]
    [HarmonyPatch("RemovePlayer")]
    public static void RemovePlayerPatch(GameData __instance, [HarmonyArgument(0)] byte playerId)
    {
        var customData = __instance.gameObject.GetComponent<CustomGameData>();
        customData.CustomPlayerInfos.Remove(playerId);
    }
}