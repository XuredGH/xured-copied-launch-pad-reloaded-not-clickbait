using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.API.Roles;
using Reactor.Utilities;

namespace LaunchpadReloaded.Patches.Roles;

[HarmonyPatch(typeof(RoleOptionsCollectionV07))]
public static class RoleOptionsCollectionPatch
{
    /// <summary>
    /// Set the role chance for custom Launchpad roles based on config
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("GetChancePerGame")]
    public static bool GetChancePrefix([HarmonyArgument(0)] RoleTypes roleType, ref int __result)
    {
        if (CustomRoleManager.GetCustomRoleBehaviour(roleType, out var customRole))
        {
            PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.TryGetEntry<int>(customRole.ChanceConfigDefinition, out var entry);
            __result = entry.Value;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Set the amount for custom Launchpad roles based on config
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("GetNumPerGame")]
    public static bool GetNumPrefix([HarmonyArgument(0)] RoleTypes roleType, ref int __result)
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
