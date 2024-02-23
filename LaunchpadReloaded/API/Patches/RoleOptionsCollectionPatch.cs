using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.API.Roles;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(RoleOptionsCollectionV07))]
public static class RoleOptionsCollectionPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(RoleOptionsCollectionV07.GetChancePerGame))]
    public static bool GetChancePrefix([HarmonyArgument(0)]RoleTypes roleType, ref int __result)
    {
        if (CustomRoleManager.GetCustomRoleBehaviour(roleType, out var customRole))
        {
            __result = 100;
            return false;
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(RoleOptionsCollectionV07.GetNumPerGame))]
    public static bool GetNumPrefix([HarmonyArgument(0)]RoleTypes roleType, ref int __result)
    {
        if (CustomRoleManager.GetCustomRoleBehaviour(roleType, out var customRole))
        {
            __result = 1;
            return false;
        }

        return true;
    }
}
