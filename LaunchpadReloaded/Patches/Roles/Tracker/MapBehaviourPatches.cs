using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Roles.Tracker;

[HarmonyPatch(typeof(MapBehaviour))]
public class MapBehaviourPatches
{
    public static SpriteRenderer TrackerPoint;

    /// <summary>
    /// Only show sabotage map if enabled
    /// </summary>
    [HarmonyPrefix, HarmonyPatch(nameof(MapBehaviour.ShowSabotageMap))]
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

    /// <summary>
    /// Only show map if click is not cancelled
    /// </summary>
    [HarmonyPrefix, HarmonyPatch(nameof(MapBehaviour.Show))]
    public static bool MapBehaviourShowPrefix()
    {
        return !Helpers.ShouldCancelClick();
    }

    /// <summary>
    /// Add tracked player icon on map
    /// </summary>
    [HarmonyPostfix, HarmonyPatch(nameof(MapBehaviour.ShowNormalMap))]
    public static void ShowMapPatch(MapBehaviour __instance)
    {
        if (PlayerControl.LocalPlayer.Data.Role is TrackerRole && TrackingManager.Instance.TrackedPlayer && !TrackerPoint)
        {
            TrackerPoint = Object.Instantiate(__instance.HerePoint, __instance.HerePoint.transform.parent);
            TrackerPoint.name = "TrackingPlayer";
            TrackerPoint.transform.localPosition = TrackingManager.Instance.MapPosition;
            TrackingManager.Instance.TrackedPlayer.SetPlayerMaterialColors(TrackerPoint);
        }
    }

    /// <summary>
    /// Update the tracked player if exists
    /// </summary>
    [HarmonyPostfix, HarmonyPatch(nameof(MapBehaviour.FixedUpdate))]
    public static void UpdatePatch(MapBehaviour __instance)
    {
        if (PlayerControl.LocalPlayer.Data.Role is TrackerRole && TrackingManager.Instance.TrackedPlayer && TrackerPoint != null)
        {
            TrackerPoint.transform.localPosition = Vector3.Lerp(TrackerPoint.transform.localPosition, TrackingManager.Instance.MapPosition, Time.deltaTime * 1.2f);
        }
    }
}