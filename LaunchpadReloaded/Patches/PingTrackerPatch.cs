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
        __instance.gameObject.SetActive(true);
        __instance.text.text =
            $"<align=\"center\"><color=#FF4050FF>All Of Us:</color> Launchpad \n<size=90%><color=#7785CC>dsc.gg/allofus</color>\n<size=80%>{__instance.text.text}";
        
        var aspectPos = __instance.GetComponent<AspectPosition>();
        var x = HudManager.Instance.GetComponentInChildren<FriendsListButton>() ? 3.5f : 2f;
        aspectPos.DistanceFromEdge = new Vector3(x, 0.1f, 0);
    }
}