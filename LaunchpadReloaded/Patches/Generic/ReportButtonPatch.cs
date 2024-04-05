using HarmonyLib;
using LaunchpadReloaded.Features.Managers;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(ReportButton))]
public static class ReportButtonPatch
{
    [HarmonyPrefix, HarmonyPatch("DoClick")]
    public static bool DoClickPatch()
    {
        if (HackingManager.Instance is null)
        {
            return true;
        }

        return !HackingManager.Instance.AnyPlayerHacked();
    }
}