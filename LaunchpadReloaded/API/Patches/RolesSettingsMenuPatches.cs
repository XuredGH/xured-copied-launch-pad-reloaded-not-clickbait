using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.API.Roles;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(RolesSettingsMenu))]
public static class RolesSettingsMenuPatches
{
    public static List<RoleOptionSetting> Options = new();
    
    [HarmonyPrefix]
    [HarmonyPatch("OnEnable")]
    public static void OnEnablePrefix(RolesSettingsMenu __instance)
    {
        var parent = __instance.AllRoleSettings.ToArray()[0].transform.parent;
        foreach (var (key, role) in CustomRoleManager.CustomRoles)
        {
            if (__instance.AllRoleSettings.ToArray().Any(x => (ushort)x.Role.Role == key)) continue;
            var newOption = Object.Instantiate(__instance.SettingPrefab, parent);
            newOption.Role = role;
            __instance.AllRoleSettings.Add(newOption);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch("ValueChanged")]
    public static bool ValueChangedPrefix(RolesSettingsMenu __instance, [HarmonyArgument(0)] OptionBehaviour obj)
    {
        if (obj is RoleOptionSetting { Role: ICustomRole role } roleSetting)
        {
            Debug.LogError("SETTING ROLE CONFIG");
            PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.TryGetEntry<int>(role.NumConfigDefinition, out var numEntry);
            numEntry.Value = roleSetting.RoleMaxCount;
            PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.TryGetEntry<int>(role.ChanceConfigDefinition, out var chanceEntry);
            chanceEntry.Value = roleSetting.RoleChance;
            roleSetting.UpdateValuesAndText(GameOptionsManager.Instance.CurrentGameOptions.RoleOptions);
            GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.CurrentGameOptions;
            return false;
        }

        return true;
    }
}