using HarmonyLib;
using UnityEngine;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(PingTracker))]
public static class WatermarkPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PingTracker.Update))]
    public static bool Prefix(PingTracker __instance)
    {
        var aspectPos = __instance.GetComponent<AspectPosition>();

        __instance.gameObject.SetActive(true);
        __instance.text.richText = true;
        __instance.text.text = "<color=#FF4050FF>All Of Us:</color> Launchpad \n<color=#7785CC>dsc.gg/allofus</color>";

        var x = HudManager.Instance.gameObject.GetComponentInChildren<FriendsListButton>() != null ? 4 : 2.3f;
        aspectPos.DistanceFromEdge = new Vector3(x, 0.1f, 0);

        return false;
    }
}