using HarmonyLib;
using InnerNet;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch]
public static class ConsolePatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
    public static bool CanUsePatch(Console __instance, [HarmonyArgument(0)] NetworkedPlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started && ShipStatus.Instance)
        {
            var task = __instance.FindTask(pc.Object);

            if (task && task.GetComponent<SabotageTask>())
            {
                canUse = couldUse = true;
                return true;
            }

            if (pc.IsHacked())
            {
                return canUse = couldUse = false;
            }

            canUse = false;
            couldUse = false;
            return true;
        }

        canUse = couldUse = true;
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SystemConsole), nameof(SystemConsole.CanUse))]
    public static bool SystemCanUsePatch(SystemConsole __instance, [HarmonyArgument(0)] NetworkedPlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started && ShipStatus.Instance)
        {
            return canUse = couldUse = !HackingManager.Instance.AnyPlayerHacked();
        }

        canUse = couldUse = true;
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MapConsole), nameof(MapConsole.CanUse))]
    public static bool MapCanUsePatch(MapConsole __instance, [HarmonyArgument(0)] NetworkedPlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started && ShipStatus.Instance)
        {                
            return canUse = couldUse = !pc.IsHacked();
        }

        canUse = couldUse = true;
        return true;
    }
}