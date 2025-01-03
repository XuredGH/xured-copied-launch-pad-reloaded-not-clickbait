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
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DiscordManager), nameof(DiscordManager.Start))]
    public static bool DiscordManagerStartPrefix(DiscordManager __instance)
    {
        __instance.presence = new Discord.Discord(1217217004474339418, 1UL);
        var activityManager = __instance.presence.GetActivityManager();
        activityManager.RegisterSteam(945360U);
        activityManager.add_OnActivityJoin((Action<string>)__instance.HandleJoinRequest);
        SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>)((scene, _)=>
        {
            __instance.OnSceneChange(scene.name);
        }));
        __instance.SetInMenus();
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ActivityManager), nameof(ActivityManager.UpdateActivity))]
    public static void ActivityManagerUpdateActivityPrefix(ActivityManager __instance, [HarmonyArgument(0)] Activity activity)
    {
        activity.Details += " All Of Us: Launchpad";
        activity.State += " | dsc.gg/allofus";
    }
}