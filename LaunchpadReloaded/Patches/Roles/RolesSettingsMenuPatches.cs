using System.Linq;
using AmongUs.GameOptions;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using Reactor.Utilities.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.Patches.Roles;

[HarmonyPatch(typeof(RolesSettingsMenu))]
public static class RolesSettingsMenuPatches
{
    /// <summary>
    /// Create an advanced role settings menu for every custom Launchpad role
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("Start")]
    public static void StartPrefix(RolesSettingsMenu __instance)
    {
        var tabPrefab = __instance.AllAdvancedSettingTabs.ToArray()[1].Tab;
        foreach (var (key, role) in CustomRoleManager.CustomRoles)
        {
            if (__instance.AllAdvancedSettingTabs.ToArray().Any(x => (ushort)x.Type == key)) continue;

            var newTab = Object.Instantiate(tabPrefab, __instance.AdvancedRolesSettings.transform);
            newTab.name = role.NiceName + " Settings";
            var toggleSet = Object.Instantiate(newTab.GetComponentInChildren<ToggleOption>(true));
            var numberSet = Object.Instantiate(newTab.GetComponentInChildren<NumberOption>(true));

            foreach (var option in newTab.GetComponentsInChildren<OptionBehaviour>())
            {
                option.gameObject.Destroy();
            }

            var startOffset = 0.5f;

            foreach (var customOption in CustomOptionsManager.CustomOptions)
            {
                if (customOption.AdvancedRole is not null && customOption.AdvancedRole == role.GetType())
                {
                    startOffset -= 0.5f;

                    switch (customOption)
                    {
                        case CustomNumberOption numberOption:
                            var numOpt = numberOption.CreateNumberOption(numberSet, newTab.transform);
                            numOpt.transform.localPosition = new Vector3(-1.25f, startOffset, 0);
                            break;

                        case CustomToggleOption toggleOption:
                            var togOpt = toggleOption.CreateToggleOption(toggleSet, newTab.transform);
                            togOpt.transform.localPosition = new Vector3(-1.25f, startOffset, 0);
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

            toggleSet.gameObject.Destroy();
            numberSet.gameObject.Destroy();
        }
    }

    /// <summary>
    /// Set custom role options OnValueChanged and re-enable the role settings menu scrolling
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("Start")]
    public static void StartPostfix(RolesSettingsMenu __instance)
    {
        foreach (var customOption in CustomOptionsManager.CustomOptions)
        {
            if (customOption.AdvancedRole is not null)
            {
                customOption.OptionBehaviour.OnValueChanged = (Action<OptionBehaviour>)customOption.ValueChanged;
            }
        }

        var scroll = __instance.GetComponentInChildren<Scroller>();
        scroll.active = true;
        scroll.ContentYBounds.max = __instance.Children.Count * 0.02f;

        scroll.transform.FindChild("UI_Scrollbar").gameObject.SetActive(true);
    }

    /// <summary>
    /// Add the role settings to the menu
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("OnEnable")]
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

    /// <summary>
    /// Update config when value changed
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("ValueChanged")]
    public static bool ValueChangedPrefix(RolesSettingsMenu __instance, [HarmonyArgument(0)] OptionBehaviour obj)
    {
        if (obj.GetIl2CppType() != Il2CppType.Of<RoleOptionSetting>())
        {
            return true;
        }
        
        var roleSetting = obj.Cast<RoleOptionSetting>();
        
        if (roleSetting.Role is not ICustomRole role)
        {
            return true;
        }
        
        LaunchpadReloadedPlugin.Instance.Config.TryGetEntry<int>(role.NumConfigDefinition, out var numEntry);
        numEntry.Value = roleSetting.RoleMaxCount;

        LaunchpadReloadedPlugin.Instance.Config.TryGetEntry<int>(role.ChanceConfigDefinition, out var chanceEntry);
        chanceEntry.Value = roleSetting.RoleChance;

        roleSetting.UpdateValuesAndText(GameOptionsManager.Instance.CurrentGameOptions.RoleOptions);

        if (AmongUsClient.Instance.AmHost)
        {
            CustomRoleManager.SyncRoleSettings();
        }
        GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.CurrentGameOptions;
        return false;

    }
}