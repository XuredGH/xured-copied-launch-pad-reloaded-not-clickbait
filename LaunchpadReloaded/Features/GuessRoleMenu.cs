using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using LaunchpadReloaded.Utilities;
using MiraAPI.Patches.Stubs;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LaunchpadReloaded.Features;

[RegisterInIl2Cpp]
public sealed class GuessRoleMenu(IntPtr ptr) : Minigame(ptr)
{
    private ShapeshifterPanel panelPrefab;
    private float xStart = -0.8f;
    private float yStart = 2.15f;
    private float xOffset = 1.95f;
    private float yOffset = -0.65f;
    private UiElement backButton;
    private UiElement defaultButtonSelected;
    private Action<RoleBehaviour> onClick;
    private List<ShapeshifterPanel> potentialVictims;
    private int currentPage;

    public static GuessRoleMenu Create()
    {
        var shapeShifterRole = RoleManager.Instance.GetRole(RoleTypes.Shapeshifter);

        var ogMenu = shapeShifterRole.TryCast<ShapeshifterRole>()!.ShapeshifterMenu;
        var newMenu = Instantiate(ogMenu);
        var customMenu = newMenu.gameObject.AddComponent<GuessRoleMenu>();

        customMenu.panelPrefab = newMenu.PanelPrefab;
        customMenu.xStart = newMenu.XStart;
        customMenu.yStart = newMenu.YStart;
        customMenu.xOffset = newMenu.XOffset;
        customMenu.yOffset = newMenu.YOffset;
        customMenu.defaultButtonSelected = newMenu.DefaultButtonSelected;
        customMenu.backButton = newMenu.BackButton;
        var back = customMenu.backButton.GetComponent<PassiveButton>();
        back.OnClick.RemoveAllListeners();
        back.OnClick.AddListener((UnityAction)Instance.Close);

        customMenu.CloseSound = newMenu.CloseSound;
        customMenu.logger = newMenu.logger;
        customMenu.OpenSound = newMenu.OpenSound;

        newMenu.DestroyImmediate();

        customMenu.transform.SetParent(Camera.main.transform, false);
        customMenu.transform.localPosition = new Vector3(0f, 0f, -50f);

        var nextButton = Instantiate(customMenu.backButton, customMenu.transform).gameObject;
        nextButton.transform.localPosition = new Vector3(-1.85f, -2.185f, -50f);
        nextButton.transform.localScale = new Vector3(0.65f, 0.65f, 1);
        nextButton.name = "RightArrowButton";
        nextButton.GetComponent<SpriteRenderer>().sprite = MiraAssets.NextButton.LoadAsset();
        nextButton.gameObject.GetComponent<CloseButtonConsoleBehaviour>().DestroyImmediate();

        var passiveButton = nextButton.gameObject.GetComponent<PassiveButton>();
        passiveButton.OnClick = new Button.ButtonClickedEvent();
        passiveButton.OnClick.AddListener((UnityAction)(() =>
        {
            customMenu.currentPage++;
            if (customMenu.currentPage > Mathf.CeilToInt(customMenu.potentialVictims.Count / 15f) - 1)
            {
                customMenu.currentPage = 0;
            }

            customMenu.ShowPage();
        }));

        var backButton = Instantiate(nextButton, customMenu.transform).gameObject;
        nextButton.transform.localPosition = new Vector3(1.85f, -2.185f, -50f);
        backButton.name = "LeftArrowButton";
        backButton.gameObject.GetComponent<CloseButtonConsoleBehaviour>().Destroy();
        backButton.GetComponent<SpriteRenderer>().flipX = true;
        backButton.gameObject.GetComponent<PassiveButton>().OnClick.AddListener((UnityAction)(() =>
        {
            customMenu.currentPage--;
            if (customMenu.currentPage < 0)
            {
                customMenu.currentPage = Mathf.CeilToInt(customMenu.potentialVictims.Count / 15f) - 1;
            }

            customMenu.ShowPage();
        }));

        return customMenu;
    }

    public void OnDisable()
    {
        ControllerManager.Instance.CloseOverlayMenu(name);
    }

    public Il2CppSystem.Collections.Generic.List<UiElement> ShowPage()
    {
        foreach (var panel in potentialVictims)
        {
            panel.gameObject.SetActive(false);
        }

        var list = potentialVictims.Skip(currentPage * 15).Take(15).ToList();
        var list2 = new Il2CppSystem.Collections.Generic.List<UiElement>();

        foreach (var panel in list)
        {
            panel.gameObject.SetActive(true);
            list2.Add(panel.Button);
        }

        return list2;
    }

    [HideFromIl2Cpp]
    public void Begin(Func<RoleBehaviour, bool> roleMatch, Action<RoleBehaviour> clickHandler)
    {
        MinigameStubs.Begin(this, null);

        onClick = clickHandler;
        potentialVictims = [];

        var roles = RoleManager.Instance.AllRoles.Where(role => roleMatch(role)).ToArray();

        for (var i = 0; i < roles.Count(); i++)
        {
            var role = roles[i];
            var num = i % 3;
            var num2 = (i / 3) % 5;
            var shapeshifterPanel = Instantiate(this.panelPrefab, this.transform);
            shapeshifterPanel.transform.localPosition = new Vector3(this.xStart + num * this.xOffset, this.yStart + num2 * this.yOffset, -1f);
            shapeshifterPanel.SetRole(i, role, () => { onClick(role); });
            this.potentialVictims.Add(shapeshifterPanel);
        }

        var list2 = ShowPage();

        ControllerManager.Instance.OpenOverlayMenu(this.name, this.backButton, this.defaultButtonSelected, list2);
    }
}