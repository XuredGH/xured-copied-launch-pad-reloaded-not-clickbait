﻿using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadReloaded.API.Patches;


[HarmonyPatch]
public static class ConsolePatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
    public static bool CanUsePatch(Console __instance, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        if(HackButton.HackedPlayers.Contains(pc) & !__instance.TaskTypes.Contains(TaskTypes.FixComms)) {
            canUse = false;
            couldUse = false;
            return false; 
        }

        canUse = true;
        couldUse = true;
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SystemConsole), nameof(SystemConsole.CanUse))]
    public static bool SystemCanUsePatch([HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        if (HackButton.HackedPlayers.Contains(pc))
        {
            canUse = false;
            couldUse = false;
            return false;
        }

        canUse = true;
        couldUse = true;
        return true;
    }
}
