using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(Vent))]
public static class VentPatches
{
    [HarmonyPrefix]
    [HarmonyPatch("CanUse")]
    public static bool CanUsePatch(Vent __instance, [HarmonyArgument(0)] GameData.PlayerInfo playerInfo, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        var canVent = CustomGamemodeManager.ActiveMode.CanVent(__instance, playerInfo);
        canUse = couldUse = canVent;
        return canVent;
    }

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