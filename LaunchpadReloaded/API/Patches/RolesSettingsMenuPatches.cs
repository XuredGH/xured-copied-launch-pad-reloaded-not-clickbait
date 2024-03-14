using System.Linq;
using AmongUs.GameOptions;
using HarmonyLib;
using Il2CppSystem;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using Reactor.Utilities.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(RolesSettingsMenu))]
public static class RolesSettingsMenuPatches
{

    // TODO: Add advanced role option settings
    [HarmonyPrefix]
    [HarmonyPatch("Start")]
    public static void StartPrefix(RolesSettingsMenu __instance)
    {
        var tabPrefab = __instance.AllAdvancedSettingTabs.ToArray()[1].Tab;
        foreach (var (key, role) in CustomRoleManager.CustomRoles)
        {
            if (__instance.AllAdvancedSettingTabs.ToArray().Any(x => (ushort)x.Type == key))
            {
                continue;
            }

            var newTab = Object.Instantiate(tabPrefab, __instance.AdvancedRolesSettings.transform);
            newTab.name = role.NiceName + " Settings";
            var toggleSet = Object.Instantiate(newTab.GetComponentInChildren<ToggleOption>(true));
            var numberSet = Object.Instantiate(newTab.GetComponentInChildren<NumberOption>(true));

            foreach (var option in newTab.GetComponentsInChildren<OptionBehaviour>())
            {
                option.gameObject.Destroy();
            }

            var numOptsAdded = 0;
            foreach (var customOption in CustomOptionsManager.CustomOptions)
            {
                if (customOption.AdvancedRole is not null && customOption.AdvancedRole == role.GetType())
                {
                    switch (customOption)
                    {
                        case CustomNumberOption numberOption:
                            var numOpt = Object.Instantiate(numberSet, newTab.transform);
                            numOpt.transform.localPosition -= new Vector3(0, .5f * numOptsAdded++, 0);
                            numberOption.CreateNumberOption(numOpt);

                            break;

                        case CustomToggleOption toggleOption:
                            var togOpt = Object.Instantiate(toggleSet, newTab.transform);
                            togOpt.transform.localPosition -= new Vector3(0, .5f * numOptsAdded++, 0);
                            toggleOption.CreateToggleOption(togOpt);

                            break;
                    }
                }
            }

            var tmp = newTab.GetComponentInChildren<TextTranslatorTMP>();
            tmp.defaultStr = role.NiceName;
            tmp.TargetText = role.StringName;

            var newAdvSet = new AdvancedRoleSettingsButton
            {
                Tab = newTab,
                Type = (RoleTypes)key
            };

            __instance.AllAdvancedSettingTabs.Add(newAdvSet);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("Start")]
    public static void StartPostfix(RolesSettingsMenu __instance)
    {
        foreach (var customOption in CustomOptionsManager.CustomOptions)
        {
            if (customOption.AdvancedRole is not null)
            {
                customOption.OptionBehaviour.OnValueChanged = (Action<OptionBehaviour>)customOption.ValueChanged;
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch("OnEnable")]
    public static void OnEnablePrefix(RolesSettingsMenu __instance)
    {
        var parent = __instance.ItemParent;
        foreach (var (key, role) in CustomRoleManager.CustomRoles)
        {
            if (__instance.AllRoleSettings.ToArray().Any(x => (ushort)x.Role.Role == key))
            {
                continue;
            }

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
            LaunchpadReloadedPlugin.Instance.Config.TryGetEntry<int>(role.NumConfigDefinition, out var numEntry);
            numEntry.Value = roleSetting.RoleMaxCount;

            LaunchpadReloadedPlugin.Instance.Config.TryGetEntry<int>(role.ChanceConfigDefinition, out var chanceEntry);
            chanceEntry.Value = roleSetting.RoleChance;

            roleSetting.UpdateValuesAndText(GameOptionsManager.Instance.CurrentGameOptions.RoleOptions);

            GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.CurrentGameOptions;
            return false;
        }

        return true;
    }
}