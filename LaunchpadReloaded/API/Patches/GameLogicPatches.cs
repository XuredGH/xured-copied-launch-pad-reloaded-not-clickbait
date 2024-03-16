using HarmonyLib;
using LaunchpadReloaded.API.GameModes;

namespace LaunchpadReloaded.API.Patches;
[HarmonyPatch]
public static class GameLogicPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
    public static bool EndCritPatch(LogicGameFlowNormal __instance)
    {
        CustomGameModeManager.ActiveMode.CheckGameEnd(out var runOriginal, __instance);
        return runOriginal;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LogicRoleSelectionNormal), nameof(LogicRoleSelectionNormal.AssignRolesFromList))]
    public static bool AssignRolesPatch(LogicRoleSelectionNormal __instance)
    {
        CustomGameModeManager.ActiveMode.AssignRoles(out var runOriginal, __instance);
        return runOriginal;
    }
}