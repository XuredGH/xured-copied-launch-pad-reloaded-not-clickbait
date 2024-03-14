using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.API.GameOptions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(GameSettingMenu))]
public static class GameSettingsMenuPatches
{
    public static GameObject gameBtn;
    public static GameObject roleBtn;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(GameSettingMenu.Start))]
    public static void StartPrefix(GameSettingMenu __instance)
    {
        if (CustomOptionsTab.customTab != null) return;
        __instance.Tabs.transform.position += new Vector3(0.5f, 0, 0);
        gameBtn = __instance.transform.FindChild("Header/Tabs/GameTab").gameObject;
        roleBtn = __instance.transform.FindChild("Header/Tabs/RoleTab").gameObject;

        var numberOpt = __instance.RegularGameSettings.GetComponentInChildren<NumberOption>();
        var toggleOpt = Object.FindObjectOfType<ToggleOption>();
        var stringOpt = __instance.RegularGameSettings.GetComponentInChildren<StringOption>();
        var container = CustomOptionsTab.Initialize(__instance).transform;


        foreach (CustomOptionGroup group in CustomOptionsManager.CustomGroups.Where(group => group.AdvancedRole == null))
        {
            group.Header = CustomOptionsTab.CreateHeader(toggleOpt, container, group.Title);
            CreateOptionsFor(__instance, toggleOpt, numberOpt, stringOpt, container,
                group.CustomToggleOptions, group.CustomNumberOptions, group.CustomStringOptions);
        }

        CreateOptionsFor(__instance, toggleOpt, numberOpt, stringOpt, container,
            CustomOptionsManager.CustomToggleOptions.Where(option => option.Group == null),
            CustomOptionsManager.CustomNumberOptions.Where(option => option.Group == null),
            CustomOptionsManager.CustomStringOptions.Where(option => option.Group == null));

        if (!numberOpt || !toggleOpt || !stringOpt)
        {
            Debug.LogError("OPTION PREFABS MISSING");
            return;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameSettingMenu.Update))]
    public static void UpdatePatch(GameSettingMenu __instance)
    {
        if (CustomGamemodeManager.ActiveMode == null) return;

        gameBtn.SetActive(CustomGamemodeManager.ActiveMode.CanAccessSettingsTab());
        roleBtn.SetActive(CustomGamemodeManager.ActiveMode.CanAccessRolesTab());

        if (!CustomGamemodeManager.ActiveMode.CanAccessSettingsTab())
        {
            __instance.RegularGameSettings.SetActive(false);
            __instance.RolesSettings.gameObject.SetActive(false);

            CustomOptionsTab.customScreen.gameObject.SetActive(true);
            CustomOptionsTab.rend.enabled = true;

            __instance.GameSettingsHightlight.enabled = false;
            __instance.RolesSettingsHightlight.enabled = false;
        }
    }

    public static void CreateOptionsFor(GameSettingMenu __instance, ToggleOption toggleOpt, NumberOption numberOpt, StringOption stringOpt, Transform container,
        IEnumerable<CustomToggleOption> toggles, IEnumerable<CustomNumberOption> numbers, IEnumerable<CustomStringOption> strings)
    {
        foreach (var customToggleOption in toggles)
        {
            if (customToggleOption.AdvancedRole is not null || (customToggleOption.OptionBehaviour)) continue;

            var newOpt = Object.Instantiate(toggleOpt, container);
            customToggleOption.CreateToggleOption(newOpt);
            __instance.AllItems = __instance.AllItems.AddItem(newOpt.transform).ToArray();
        }

        foreach (var customNumberOption in numbers)
        {
            if (customNumberOption.AdvancedRole is not null || (customNumberOption.OptionBehaviour)) continue;

            var newOpt = Object.Instantiate(numberOpt, container);
            customNumberOption.CreateNumberOption(newOpt);
            __instance.AllItems = __instance.AllItems.AddItem(newOpt.transform).ToArray();
        }

        foreach (var customStringOption in strings)
        {
            if (customStringOption.AdvancedRole is not null || (customStringOption.OptionBehaviour)) continue;

            var newOpt = Object.Instantiate(stringOpt, container);
            customStringOption.CreateStringOption(newOpt);
            __instance.AllItems = __instance.AllItems.AddItem(newOpt.transform).ToArray();
        }
    }
}