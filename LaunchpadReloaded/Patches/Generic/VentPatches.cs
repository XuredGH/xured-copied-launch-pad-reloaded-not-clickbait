using HarmonyLib;
using LaunchpadReloaded.Components;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(Vent))]
public static class VentPatches
{

    /// <summary>
    /// Expose hidden bodies when venting
    /// </summary>
    [HarmonyPatch(nameof(Vent.EnterVent))]
    [HarmonyPatch(nameof(Vent.ExitVent))]
    public static void Postfix(Vent __instance)
    {
        var ventBody = __instance.GetComponent<VentBodyComponent>();
        if (ventBody && ventBody.deadBody)
        {
            ventBody.ExposeBody();
        }
    }
}