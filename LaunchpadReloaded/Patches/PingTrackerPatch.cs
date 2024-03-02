using HarmonyLib;
using UnityEngine;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(PingTracker))]
public static class PingTrackerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PingTracker.Update))]
    public static void Postfix(PingTracker __instance)
    {
        __instance.text.text += "\n<align=\"center\"><color=#FF4050FF>All Of Us:</color> Launchpad \n<color=#7785CC>dsc.gg/allofus</color>";
    }
}