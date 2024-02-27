using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        CustomOptions.Add((CustomOption)Activator.CreateInstance(buttonType));
    }

    private static OptionsMenuBehaviour optionsMenu;

    public static void Start(OptionsMenuBehaviour optionsMenu)
    {
        CustomOptionsManager.optionsMenu = optionsMenu;
        CreateTab();
    }

    public static void CreateTab() 
    {
        // replace help tab (its unusued in game iirc)
        foreach (TabGroup tab in optionsMenu.Tabs)
        {
            var tabButton = tab.gameObject;
            tabButton.transform.localPosition -= new Vector3(0.8f, 0, 0);
        }

        var newTabButton = optionsMenu.Tabs.Last();
        var newTabButtonText = newTabButton.GetComponentInChildren<TextMeshPro>();
        GameObject.Destroy(newTabButton.GetComponentInChildren<TextTranslatorTMP>());
        newTabButton.name = "LaunchpadButton";
        newTabButtonText.text = "Launchpad";
        newTabButton.gameObject.SetActive(true);
        newTabButton.transform.localPosition += new Vector3(0.93f, 0, 0);

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
        foreach (CustomOption customOption in CustomOptions)
        {
            transforms.Add(customOption.CreateButton(optionsMenu, LaunchpadTab.transform).transform);
        }

        aspectPosition.updateAlways = true;
        aspectPosition.DistanceFromEdge = new Vector3(1.3f, 1f, 0);
        gridArrange.Start();
        gridArrange.ArrangeChilds();

        aspectPosition.AdjustPosition();
    }
}