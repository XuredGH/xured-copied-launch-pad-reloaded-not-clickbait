using HarmonyLib;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

/// <summary>
/// Launchpad watermark
/// </summary>
[HarmonyPatch(typeof(PingTracker))]
public static class PingTrackerPatch
{
    [HarmonyPrefix, HarmonyPatch("Update")]
    public static bool Prefix(PingTracker __instance)
    {
        var aspectPos = __instance.GetComponent<AspectPosition>();

        __instance.gameObject.SetActive(true);
        __instance.text.richText = true;
        __instance.text.text = $"<color=#FF4050FF>All Of Us:</color> Launchpad \n<color=#7785CC>dsc.gg/allofus</color>\n<size=50%>Ping: {AmongUsClient.Instance.Ping} ms</size>";

        var x = HudManager.Instance.gameObject.GetComponentInChildren<FriendsListButton>() != null ? 4 : 2.3f;
        aspectPos.DistanceFromEdge = new Vector3(x, 0.1f, 0);

        return false;
    }
}