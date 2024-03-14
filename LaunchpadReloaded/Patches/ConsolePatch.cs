using HarmonyLib;
using InnerNet;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches;


[HarmonyPatch]
public static class ConsolePatch
{

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
    public static bool CanUsePatch(Console __instance, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started && ShipStatus.Instance)
        {
            if (CustomGamemodeManager.ActiveMode.CanUseConsole(__instance))
            {
                if (pc.IsHacked())
                {
                    return canUse = couldUse = false;
                }

                var task = __instance.FindTask(pc.Object);

                if (task && task.GetComponent<SabotageTask>())
                {
                    return canUse = couldUse = false;

                }

                canUse = false;
                couldUse = false;
                return true;
            }

            canUse = couldUse = false;
            return false;
        }

        canUse = couldUse = true;
        return true;
    }

    [HarmonyPatch(typeof(MapConsole), nameof(MapConsole.CanUse))]
    public static bool SystemCanUsePatch([HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        return canUse = couldUse = !HackingManager.Instance.AnyActiveNodes();
    }
}
