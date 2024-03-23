using System;
using Discord;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace LaunchpadReloaded.Patches.Generic;

/// <summary>
/// Custom Discord RPC
/// </summary>
[HarmonyPatch]
public static class DiscordManagerPatch
{
    [HarmonyPrefix, HarmonyPatch(typeof(DiscordManager), "Start")]
    public static bool StartPatch(DiscordManager __instance)
    {
        __instance.presence = new Discord.Discord(1217217004474339418, 1UL);
        var activityManager = __instance.presence.GetActivityManager();
        activityManager.RegisterSteam(945360U);
        activityManager.OnActivityJoin = (Action<string>)__instance.HandleJoinRequest;
        SceneManager.sceneLoaded = (Action<Scene, LoadSceneMode>)delegate (Scene scene, LoadSceneMode _)
        {
            __instance.OnSceneChange(scene.name);
        };
        __instance.SetInMenus();
        return false;
    }

    [HarmonyPrefix, HarmonyPatch(typeof(ActivityManager), "UpdateActivity")]
    public static void Prefix(ActivityManager __instance, [HarmonyArgument(0)] Activity activity)
    {
        activity.Details += "All Of Us: Launchpad | Modded Among Us";
        activity.State += " | dsc.gg/allofus";
    }
}