using System;
using Discord;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace LaunchpadReloaded.Patches;
[HarmonyPatch]
public static class DiscordManagerPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DiscordManager), nameof(DiscordManager.Start))]
    public static bool StartPatch(DiscordManager __instance)
    {
        __instance.presence = new Discord.Discord(1217217004474339418, 1UL);
        var activityManager = __instance.presence.GetActivityManager();
        activityManager.RegisterSteam(945360U);
        activityManager.OnActivityJoin = (Action<string>)delegate (string joinSecret)
        {
            __instance.HandleJoinRequest(joinSecret);
        };
        SceneManager.sceneLoaded = (Action<Scene, LoadSceneMode>)delegate (Scene scene, LoadSceneMode mode)
        {
            __instance.OnSceneChange(scene.name);
        };
        __instance.SetInMenus();
        return false;
    }

    [HarmonyPatch(typeof(ActivityManager), nameof(ActivityManager.UpdateActivity))]
    [HarmonyPrefix]
    public static void Prefix(ActivityManager __instance, [HarmonyArgument(0)] Activity activity)
    {
        activity.Details += "All Of Us: Launchpad | Modded Among Us";
        activity.State += " | dsc.gg/allofus";
    }
}