using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadReloaded.API.Patches;
[HarmonyPatch]
public static class GameLogicPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
    public static bool EndCritPatch(LogicGameFlowNormal __instance)
    {
        CustomGamemodeManager.ActiveMode.CheckGameEnd(out bool runOriginal, __instance);
        return runOriginal;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LogicRoleSelectionNormal), nameof(LogicRoleSelectionNormal.AssignRolesFromList))]
    public static bool AssignRolesPatch(LogicRoleSelectionNormal __instance, [HarmonyArgument(0)] List<GameData.PlayerInfo> players)
    {
        CustomGamemodeManager.ActiveMode.AssignRoles(out bool runOriginal, players, __instance);
        return runOriginal;
    }
}