using AmongUs.GameOptions;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.API.Patches;


[HarmonyPatch]
public static class ConsolePatch
{

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
    public static bool CanUsePatch(Console __instance, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        if (pc.IsHacked()) return canUse = couldUse = false;

        var task = __instance.FindTask(pc.Object);

        if (task && task.GetComponent<SabotageTask>())
        {
            return canUse = couldUse = false;

        }

        canUse = false;
        couldUse = false;
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SystemConsole), nameof(SystemConsole.CanUse))]
    public static bool SystemCanUsePatch([HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        return canUse = couldUse = !pc.IsHacked();
    }
}
