using HarmonyLib;
using LaunchpadReloaded.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadReloaded.API.Patches;

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