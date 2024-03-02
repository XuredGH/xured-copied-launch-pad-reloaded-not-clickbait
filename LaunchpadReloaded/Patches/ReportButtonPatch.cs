using HarmonyLib;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(ReportButton))]
public static class ReportButtonPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ReportButton.DoClick))]
    public static bool DoClickPatch()
    {
        return !HackingManager.AnyActiveNodes();
    }
}