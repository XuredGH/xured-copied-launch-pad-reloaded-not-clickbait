using HarmonyLib;

namespace LaunchpadReloaded.Patches.Generic;

/// <summary>
/// Launchpad watermark
/// </summary>
[HarmonyPatch(typeof(PingTracker))]
public static class PingTrackerPatch
{
    [HarmonyPostfix, HarmonyPatch("Update")]
    public static void PingUpdatePostfix(PingTracker __instance)
    {
        __instance.gameObject.SetActive(true);
        __instance.text.text = "<align=\"center\">" + __instance.text.text + $"\n<size=50%>Launchpad <color=#FF4050FF>v{LaunchpadReloadedPlugin.GetShortHashVersion()}</color>";
    }
}