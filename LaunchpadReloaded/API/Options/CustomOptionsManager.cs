﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactor.Localization.Utilities;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;

namespace LaunchpadReloaded.API.Options;
public static class CustomOptionsManager
{
    public static readonly List<CustomOption> CustomOptions = new();
    public static GameObject LaunchpadTab;

    public static void RegisterCustomOption(Type buttonType)
    {
        if (!typeof(CustomOption).IsAssignableFrom(buttonType))
        {
            return;
        }

        var opt = (CustomOption)Activator.CreateInstance(buttonType);
        opt.StringName = CustomStringName.CreateAndRegister(opt.Text);
        CustomOptions.Add(opt);
    }

    private static OptionsMenuBehaviour _optionsMenu;

    public static void Start(OptionsMenuBehaviour optMenu)
    {
        _optionsMenu = optMenu;
        CreateTab();
    }

    public static void CreateTab() 
    {
        // replace help tab (its unusued in game iirc)

        var startX = HudManager.InstanceExists ? -1.65f : -2.45f;
        var yOffset = HudManager.InstanceExists ? .1f : 0;
        for (int i = 0; i < _optionsMenu.Tabs.Count; i++)
        {
            var pos = _optionsMenu.Tabs[i].transform.localPosition; 
            _optionsMenu.Tabs[i].transform.localPosition = new Vector3(startX + i*1.65f, pos.y+yOffset, pos.z);
        }

        var newTabButton = _optionsMenu.Tabs.Last();
        var newTabButtonText = newTabButton.GetComponentInChildren<TextMeshPro>();
        newTabButton.GetComponentInChildren<TextTranslatorTMP>().Destroy();
        newTabButton.name = "LaunchpadButton";
        newTabButtonText.text = "Launchpad";
        newTabButton.gameObject.SetActive(true);

        LaunchpadTab = newTabButton.Content;
        LaunchpadTab.name = "LaunchpadTab";
        LaunchpadTab.transform.DestroyChildren();
        var gridArrange = LaunchpadTab.AddComponent<GridArrange>();
        var aspectPosition = LaunchpadTab.AddComponent<AspectPosition>();
        gridArrange.Alignment = GridArrange.StartAlign.Left;
        aspectPosition.Alignment = AspectPosition.EdgeAlignments.Top;
        gridArrange.CellSize = new Vector2(2.6f, 0.5f);
        gridArrange.MaxColumns = 2;

        var transforms = new List<Transform>();
        foreach (var customOption in CustomOptions)
        {
            transforms.Add(customOption.CreateButton(_optionsMenu, LaunchpadTab.transform).transform);
        }

        aspectPosition.updateAlways = true;
        aspectPosition.DistanceFromEdge = new Vector3(1.3f, 1f, 0);
        gridArrange.Start();
        gridArrange.ArrangeChilds();

        aspectPosition.AdjustPosition();
    }
}