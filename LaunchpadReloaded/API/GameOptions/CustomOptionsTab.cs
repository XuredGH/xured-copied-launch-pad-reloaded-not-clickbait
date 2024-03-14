using System;
using LaunchpadReloaded.API.Utilities;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.API.GameOptions;
public static class CustomOptionsTab
{
    public static GameObject customTab;
    public static GameObject customScreen;

    public static GameObject Initialize(GameSettingMenu __instance)
    {
        GameObject gameBtn = __instance.transform.FindChild("Header/Tabs/GameTab").gameObject;
        GameObject roleBtn = __instance.transform.FindChild("Header/Tabs/RoleTab").gameObject;

        customScreen = CreateNewMenu(__instance);
        customTab = CreateCustomTab(__instance, customScreen, gameBtn, roleBtn);

        SpriteRenderer rend = customTab.transform.FindChild("LaunchpadBtn/Tab Background").GetComponent<SpriteRenderer>();
        rend.enabled = false;

        UpdateListeners(__instance, gameBtn.GetComponentInChildren<PassiveButton>(), roleBtn.GetComponentInChildren<PassiveButton>(), rend);

        Transform container = customScreen.transform.FindChild("GameGroup/SliderInner");
        container.DestroyChildren();
        CreateNewResetButton(__instance, container);

        return container.gameObject;
    }

    private static void CreateNewResetButton(GameSettingMenu __instance, Transform container)
    {
        Transform resetBtn = __instance.RegularGameSettings.transform.FindChild("GameGroup/SliderInner/ResetToDefault");
        GameObject newResetBtn = GameObject.Instantiate(resetBtn.gameObject, container);
        newResetBtn.gameObject.name = "LaunchpadReset";

        TextMeshPro tmp = newResetBtn.GetComponentInChildren<TextMeshPro>();
        tmp.text = "Reset Options";
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.gameObject.transform.localPosition = new Vector3(0, 0, 0);

        newResetBtn.GetComponent<ToggleOption>().Destroy();
        newResetBtn.transform.FindChild("CheckBox").gameObject.Destroy();

        PassiveButton toggle = newResetBtn.GetComponent<PassiveButton>();

        toggle.OnClick.RemoveAllListeners();
        toggle.OnMouseOver.RemoveAllListeners();
        toggle.OnMouseOut.RemoveAllListeners();
        toggle.OnClick.AddListener((UnityAction)(() => { CustomOptionsManager.ResetToDefault(); }));
        toggle.OnMouseOver.AddListener((UnityAction)(() => { tmp.text = "<b>Reset Options</b>"; }));
        toggle.OnMouseOut.AddListener((UnityAction)(() => { tmp.text = "Reset Options"; }));
    }

    private static void UpdateListeners(GameSettingMenu __instance, PassiveButton gameB, PassiveButton roleB, SpriteRenderer rend)
    {
        gameB.OnClick.RemoveAllListeners();
        gameB.OnClick.AddListener((UnityAction)(() =>
        {
            __instance.RegularGameSettings.SetActive(true);
            __instance.RolesSettings.gameObject.SetActive(false);

            customScreen.gameObject.SetActive(false);
            rend.enabled = false;

            __instance.GameSettingsHightlight.enabled = true;
            __instance.RolesSettingsHightlight.enabled = false;
        }));

        roleB.OnClick.RemoveAllListeners();
        roleB.OnClick.AddListener((UnityAction)(() =>
        {
            __instance.RegularGameSettings.SetActive(false);
            __instance.RolesSettings.gameObject.SetActive(true);

            customScreen.gameObject.SetActive(false);
            rend.enabled = false;

            __instance.GameSettingsHightlight.enabled = false;
            __instance.RolesSettingsHightlight.enabled = true;
        }));
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
        Action value = () =>
        {
            __instance.RegularGameSettings.SetActive(false);
            __instance.RolesSettings.gameObject.SetActive(false);
            newSettings.gameObject.SetActive(true);

            SpriteRenderer rend = inside.transform.FindChild("Tab Background").GetComponent<SpriteRenderer>();
            rend.enabled = true;

            gameTab.transform.FindChild("ColorButton/Tab Background").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            roleTab.transform.FindChild("Hat Button/Tab Background").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        };
        btn.OnClick.AddListener(value);

        SpriteRenderer spriteRend = inside.GetComponentInChildren<SpriteRenderer>();
        spriteRend.sprite = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Hack.png");
        spriteRend.gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); ;

        return newTab;
    }

    public static GameObject CreateNewMenu(GameSettingMenu __instance)
    {
        GameObject gameSettings = __instance.RegularGameSettings;
        GameObject newSettings = GameObject.Instantiate(gameSettings, gameSettings.transform.parent);
        newSettings.name = "Launchpad Settings";
        newSettings.SetActive(false);

        GameObject launchpadGroup = newSettings.transform.FindChild("GameGroup").gameObject;
        TextMeshPro text = launchpadGroup.transform.FindChild("Text").gameObject.GetComponent<TextMeshPro>();
        text.gameObject.GetComponent<TextTranslatorTMP>().Destroy();
        text.text = "Launchpad Settings";

        return newSettings;
    }
}