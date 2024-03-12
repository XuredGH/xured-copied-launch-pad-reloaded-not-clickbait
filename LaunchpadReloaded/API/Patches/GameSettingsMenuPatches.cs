using HarmonyLib;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Utilities;
using Reactor.Localization.Utilities;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(GameSettingMenu),"Start")]
public static class GameSettingsMenuPatches
{
    public static GameObject customTab;
    public static GameObject customScreen;

    public static GameObject CreateCustomTab(GameSettingMenu __instance, GameObject newSettings, 
        GameObject gameTab, GameObject roleTab)
    {
        GameObject newTab = GameObject.Instantiate(gameTab, gameTab.transform.parent);
        newTab.name = "LaunchpadTab";
        gameTab.transform.position += new Vector3(-1, 0, 0);

        Transform inside = newTab.transform.FindChild("ColorButton");
        inside.name = "LaunchpadBtn";

        PassiveButton btn = inside.GetComponentInChildren<PassiveButton>();

        btn.OnClick.RemoveAllListeners();
        System.Action value = () => {
            __instance.transform.FindChild("Game Settings").gameObject.SetActive(false);
            __instance.transform.FindChild("Role Settings").gameObject.SetActive(false);
            newSettings.gameObject.SetActive(true);

            SpriteRenderer rend = inside.transform.FindChild("Tab Background").GetComponent<SpriteRenderer>();
            rend.enabled = true;

            gameTab.transform.FindChild("ColorButton/Tab Background").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            roleTab.transform.FindChild("Hat Button/Tab Background").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        };
        btn.OnClick.AddListener((UnityAction)value);

        SpriteRenderer spriteRend = inside.GetComponentInChildren<SpriteRenderer>();
        spriteRend.sprite = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Hack.png");
        spriteRend.gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); ;

        return newTab;
    }

    public static GameObject CreateNewMenu(GameSettingMenu __instance)
    {
        GameObject gameSettings = __instance.transform.FindChild("Game Settings").gameObject;
        GameObject newSettings = GameObject.Instantiate(gameSettings, gameSettings.transform.parent);
        newSettings.name = "Launchpad Settings";
        newSettings.SetActive(false);

        GameObject launchpadGroup = newSettings.transform.FindChild("GameGroup").gameObject;
        TextMeshPro text = launchpadGroup.transform.FindChild("Text").gameObject.GetComponent<TextMeshPro>();
        text.gameObject.GetComponent<TextTranslatorTMP>().Destroy();
        text.text = "Launchpad Settings";

        return newSettings;
    }

    public static void Prefix(GameSettingMenu __instance)
    {
        var numberOpt = __instance.RegularGameSettings.GetComponentInChildren<NumberOption>();
        var toggleOpt = Object.FindObjectOfType<ToggleOption>();
        var stringOpt = __instance.RegularGameSettings.GetComponentInChildren<StringOption>();

        if (!customTab)
        {
            __instance.Tabs.transform.position += new Vector3(0.5f, 0, 0);

            GameObject gameBtn = __instance.transform.FindChild("Header/Tabs/GameTab").gameObject;
            GameObject roleBtn = __instance.transform.FindChild("Header/Tabs/RoleTab").gameObject;

            customScreen = CreateNewMenu(__instance);
            customTab = CreateCustomTab(__instance, customScreen, gameBtn, roleBtn);
            SpriteRenderer rend = customTab.transform.FindChild("LaunchpadBtn/Tab Background").GetComponent<SpriteRenderer>();
            rend.enabled = false;

            PassiveButton passiveBtn = gameBtn.GetComponentInChildren<PassiveButton>();
            passiveBtn.OnClick.RemoveAllListeners();

            System.Action value = () =>
            {
                __instance.RegularGameSettings.SetActive(true);
                __instance.RolesSettings.gameObject.SetActive(false);

                customScreen.gameObject.SetActive(false);
                rend.enabled = false;

                __instance.GameSettingsHightlight.enabled = true;
                __instance.RolesSettingsHightlight.enabled = false;
            };
            passiveBtn.OnClick.AddListener((UnityAction)value);

            PassiveButton passiveBtn2 = roleBtn.GetComponentInChildren<PassiveButton>();
            passiveBtn2.OnClick.RemoveAllListeners();

            System.Action value2 = () =>
            {
                __instance.RegularGameSettings.SetActive(false);
                __instance.RolesSettings.gameObject.SetActive(true);

                customScreen.gameObject.SetActive(false);
                rend.enabled = false;

                __instance.GameSettingsHightlight.enabled = false;
                __instance.RolesSettingsHightlight.enabled = true;
            };
            passiveBtn2.OnClick.AddListener((UnityAction)value2);
        }

        Transform container = customScreen.transform.FindChild("GameGroup/SliderInner");
        container.DestroyChildren();

        foreach(CustomOptionGroup group in CustomOptionsManager.CustomGroups.Where(group => group.AdvancedRole == null))
        {
            group.Header = CreateHeader(toggleOpt, container, group.Title);
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

    public static GameObject CreateHeader(ToggleOption toggleOpt, Transform container, string title)
    {
        var header = Object.Instantiate(toggleOpt, container);

        header.Title = StringNames.None;
        header.TitleText.text = title;
        header.name = "Header";

        var transform = header.transform;
        var position = transform.localPosition;

        var checkBox = header.transform.FindChild("CheckBox")?.gameObject;
        if (checkBox) checkBox.Destroy();

        var background = header.transform.FindChild("Background")?.gameObject;
        if (background) background.Destroy();

        header.GetComponent<OptionBehaviour>().Destroy();
        return header.gameObject;
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