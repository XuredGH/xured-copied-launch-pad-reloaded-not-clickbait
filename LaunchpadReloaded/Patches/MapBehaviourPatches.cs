﻿using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(MapBehaviour))]
public class MapBehaviourPatches
{
    public static SpriteRenderer trackerHerePoint;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MapBehaviour.ShowSabotageMap))]
    public static bool ShowSabotagePatch(MapBehaviour __instance)
    {
        var shouldShow = CustomGameModeManager.ActiveMode.ShouldShowSabotageMap(__instance);
        if (!shouldShow)
        {
            __instance.ShowNormalMap();
            return false;
        }

        return true;
    }

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
        if (PlayerControl.LocalPlayer.Data.Role is TrackerRole && TrackingManager.Instance.TrackedPlayer && !trackerHerePoint)
        {
            trackerHerePoint = Object.Instantiate(__instance.HerePoint, __instance.HerePoint.transform.parent);
            trackerHerePoint.name = "TrackingPlayer";
            trackerHerePoint.transform.localPosition = TrackingManager.Instance.MapPosition;
            TrackingManager.Instance.TrackedPlayer.SetPlayerMaterialColors(trackerHerePoint);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MapBehaviour.FixedUpdate))]
    public static void UpdatePatch(MapBehaviour __instance)
    {
        if (PlayerControl.LocalPlayer.Data.Role is TrackerRole && TrackingManager.Instance.TrackedPlayer && trackerHerePoint != null)
        {
            trackerHerePoint.transform.localPosition = Vector3.Lerp(trackerHerePoint.transform.localPosition, TrackingManager.Instance.MapPosition, Time.deltaTime * 1.2f);
        }
    }
}