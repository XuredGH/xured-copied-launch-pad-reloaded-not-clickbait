using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.API.Roles;
using Reactor.Utilities;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(RoleOptionsCollectionV07))]
public static class RoleOptionsCollectionPatch
{
    //TODO: Create new options system
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(RoleOptionsCollectionV07.GetChancePerGame))]
    public static bool GetChancePrefix([HarmonyArgument(0)]RoleTypes roleType, ref int __result)
    {
        if (CustomRoleManager.GetCustomRoleBehaviour(roleType, out var customRole))
        {
            PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.TryGetEntry<int>(customRole.ChanceConfigDefinition, out var entry);
            __result = entry.Value;
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
            PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.TryGetEntry<int>(customRole.NumConfigDefinition, out var entry);
            __result = entry.Value;
            return false;
        }

        return true;
    }
}
