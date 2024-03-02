using HarmonyLib;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(MapBehaviour))]
public class MapBehaviourPatches
{
    public static SpriteRenderer trackerHerePoint;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MapBehaviour.Show))]
    public static bool MapBehaviourShowPrefix()
    {
        return !Helpers.ShouldCancelClick();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MapBehaviour.ShowNormalMap))]
    public static void ShowMapPatch(MapBehaviour __instance)
    {
        if (PlayerControl.LocalPlayer.Data.Role is TrackerRole && TrackingManager.TrackedPlayer && !trackerHerePoint)
        {
            trackerHerePoint = GameObject.Instantiate(__instance.HerePoint, __instance.HerePoint.transform.parent);
            trackerHerePoint.name = "TrackingPlayer";
            trackerHerePoint.transform.localPosition = TrackingManager.MapPosition;
            TrackingManager.TrackedPlayer.SetPlayerMaterialColors(trackerHerePoint);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MapBehaviour.FixedUpdate))]
    public static void UpdatePatch(MapBehaviour __instance)
    {
        if (PlayerControl.LocalPlayer.Data.Role is TrackerRole && TrackingManager.TrackedPlayer && trackerHerePoint != null)
        {
            trackerHerePoint.transform.localPosition = Vector3.Lerp(trackerHerePoint.transform.localPosition, TrackingManager.MapPosition, Time.deltaTime * 1.2f);
        }
    }
}