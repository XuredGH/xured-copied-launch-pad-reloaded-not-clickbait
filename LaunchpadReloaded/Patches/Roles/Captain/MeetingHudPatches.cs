using HarmonyLib;
using LaunchpadReloaded.Features;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Roles.Captain;

[HarmonyPatch]
public static class MeetingHudPatches
{
    [HarmonyPostfix, HarmonyPatch(typeof(MeetingHud), "Awake")]
    public static void AwakePostfix()
    {
        DragManager.Instance.DraggingPlayers.Clear();

        foreach (var cam in Camera.allCameras) cam.orthographicSize = 3f;
        HudManager.Instance.ShadowQuad.gameObject.SetActive(true);
        ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen);
    }
}