using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using LaunchpadReloaded.API.Roles;
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
        var originalOption = __instance.AllRoleSettings._items.FirstOrDefault();
        foreach (var (key, role) in CustomRoleManager.CustomRoles)
        {
            if (__instance.AllRoleSettings.ToArray().Any(x => (ushort)x.Role.Role == key)) continue;
            var newOption = Object.Instantiate(originalOption, originalOption.transform.parent);
            newOption.Role = role;
            __instance.AllRoleSettings.Add(newOption);
        }
        
    }

    [HarmonyPrefix]
    [HarmonyPatch("ValueChanged")]
    public static void ValueChangedPrefix(RolesSettingsMenu __instance, [HarmonyArgument(0)] OptionBehaviour obj)
    {
        if (obj is RoleOptionSetting roleSetting)
        {
            if (roleSetting.Role is ICustomRole role)
            {
                
            }
        }
    }
}