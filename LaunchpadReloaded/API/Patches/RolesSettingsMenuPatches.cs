using System.Linq;
using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.API.Roles;
using Reactor.Localization.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(RolesSettingsMenu))]
public static class RolesSettingsMenuPatches
{
    [HarmonyPrefix]
    [HarmonyPatch("Start")]
    public static void StartPrefix(RolesSettingsMenu __instance)
    {
        var tabPrefab = __instance.AllAdvancedSettingTabs.ToArray()[1].Tab;
        foreach (var (key, role) in CustomRoleManager.CustomRoles)
        {
            if (__instance.AllAdvancedSettingTabs.ToArray().Any(x => (ushort)x.Type == key)) continue;
            
            var newTab = Object.Instantiate(tabPrefab, __instance.AdvancedRolesSettings.transform);
            newTab.name = role.NiceName + " Settings";
            foreach (var option in newTab.GetComponentsInChildren<OptionBehaviour>())
            {
                option.gameObject.Destroy();
            }

            var tmp = newTab.GetComponentInChildren<TextTranslatorTMP>();
            tmp.defaultStr = role.NiceName;
            tmp.TargetText = role.StringName;
            
            var newOpt = Object.Instantiate(tabPrefab.GetComponentInChildren<NumberOption>(true),newTab.transform);
            newOpt.Title = CustomStringName.CreateAndRegister("Testing");
            
            var newAdvSet = new AdvancedRoleSettingsButton()
            {
                Tab = newTab,
                Type = (RoleTypes)key
            };
            
            __instance.AllAdvancedSettingTabs.Add(newAdvSet);
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch("OnEnable")]
    public static void OnEnablePrefix(RolesSettingsMenu __instance)
    {
        var parent = __instance.ItemParent;
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