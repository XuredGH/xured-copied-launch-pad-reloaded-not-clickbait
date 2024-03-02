using HarmonyLib;
using LaunchpadReloaded.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(DeadBody))]
public static class DeadBodyPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(DeadBody.OnClick))]
    public static bool OnClickPatch()
    {
        return !HackingManager.AnyActiveNodes();
    }
}
