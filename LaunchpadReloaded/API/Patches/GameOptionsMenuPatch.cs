using HarmonyLib;
using Il2CppSystem;
using LaunchpadReloaded.API.GameOptions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Il2CppSystem.Uri;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(GameOptionsMenu))]
public static class GameOptionsMenuPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("Start")]
    public static void StartPostfix(GameOptionsMenu __instance)
    {
        foreach (var customOption in CustomOptionsManager.CustomOptions)
        {
            if (customOption.AdvancedRole is not null) continue;
            customOption.OptionBehaviour.OnValueChanged = (Action<OptionBehaviour>)customOption.ValueChanged;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("Update")]
    public static void UpdatePostfix(GameOptionsMenu __instance)
    {
        GameSettingMenu menu = GameObject.FindObjectsOfType<GameSettingMenu>().First();
        if (menu.RegularGameSettings.active || menu.RolesSettings.gameObject.active) return;

        float startOffset = 2.75f;
        __instance.GetComponentInParent<Scroller>().ContentYBounds.max = startOffset + __instance.Children.Count * 0.5f;

        foreach (CustomOptionGroup group in CustomOptionsManager.CustomGroups.Where(group => group.AdvancedRole == null))
        {
            group.Header.SetActive(!group.Hidden());
            startOffset -= 0.5f;
            group.Header.transform.localPosition = new Vector3(group.Header.transform.localPosition.x, startOffset, group.Header.transform.localPosition.z);

            foreach (AbstractGameOption option in group.Options)
            {
                if (!option.OptionBehaviour) continue;
                option.OptionBehaviour.enabled = !group.Hidden() && !option.Hidden();
                option.OptionBehaviour.gameObject.SetActive(!group.Hidden() && !option.Hidden());

                startOffset -= 0.5f;
                option.OptionBehaviour.transform.localPosition = new Vector3(option.OptionBehaviour.transform.localPosition.x, startOffset, option.OptionBehaviour.transform.localPosition.z);
            }
        }

        foreach (AbstractGameOption option in CustomOptionsManager.CustomOptions.Where(option => option.Group == null))
        {
            if (!option.OptionBehaviour) continue;

            option.OptionBehaviour.enabled = !option.Hidden();
            option.OptionBehaviour.gameObject.SetActive(!option.Hidden());

            startOffset -= 0.5f;
            option.OptionBehaviour.transform.localPosition = new Vector3(option.OptionBehaviour.transform.localPosition.x, startOffset, option.OptionBehaviour.transform.localPosition.z);
        }
    }
}