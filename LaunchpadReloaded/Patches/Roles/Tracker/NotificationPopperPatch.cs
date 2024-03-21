using HarmonyLib;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Roles.Tracker;
/// <summary>
/// Custom "notification" system, not final and may be reworked/removed
/// </summary>
[HarmonyPatch(typeof(NotificationPopper))]
public static class NotificationPopperPatch
{
    public static NotificationPopper NewPopper;

    [HarmonyPostfix, HarmonyPatch("Update")]
    public static void UpdatePatch(NotificationPopper __instance)
    {
        if (!NewPopper)
        {
            var newGameObject = Object.Instantiate(__instance.gameObject, __instance.transform.parent);
            newGameObject.name = "LaunchpadNotifications";
            NewPopper = newGameObject.GetComponent<NotificationPopper>();
        }

        if (__instance.gameObject.name != NewPopper.gameObject.name)
        {
            return;
        }

        var pos = HudManager.Instance.TaskStuff.transform.FindChild("ProgressTracker").transform.localPosition;
        __instance.transform.localPosition = new Vector3(pos.x + 2.5f, pos.y + 0.1f, __instance.zPos);
    }
}