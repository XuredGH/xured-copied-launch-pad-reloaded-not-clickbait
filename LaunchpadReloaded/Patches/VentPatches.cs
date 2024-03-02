using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(Vent))]
public static class VentPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("EnterVent")]
    [HarmonyPatch("ExitVent")]
    public static void EnterExitPostfix(Vent __instance)
    {
        var ventBody = __instance.GetComponent<VentBodyComponent>();
        if (ventBody && ventBody.deadBody)
        {
            DeadBodyManager.RpcExposeBody(ShipStatus.Instance, __instance.Id);
        }
    }
}