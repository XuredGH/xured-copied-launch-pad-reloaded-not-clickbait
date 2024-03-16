using HarmonyLib;
using Il2CppSystem;
using LaunchpadReloaded.API.GameOptions;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

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
            if (customOption.AdvancedRole is not null)
            {
                continue;
            }

            customOption.OptionBehaviour.OnValueChanged = (Action<OptionBehaviour>)customOption.ValueChanged;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("Update")]
    public static void UpdatePostfix(GameOptionsMenu __instance)
    {
        var menu = Object.FindObjectsOfType<GameSettingMenu>().First();
        if (menu.RegularGameSettings.active || menu.RolesSettings.gameObject.active)
        {
            return;
        }

        var startOffset = 2.15f;
        __instance.GetComponentInParent<Scroller>().ContentYBounds.max = startOffset + __instance.Children.Count * 0.5f;

        foreach (var option in CustomOptionsManager.CustomOptions.Where(option => option.Group == null))
        {
            option.OptionBehaviour.enabled = !option.Hidden();
            option.OptionBehaviour.gameObject.SetActive(!option.Hidden());

            if (!option.Hidden()) startOffset -= 0.7f;
            var transform = option.OptionBehaviour.transform;
            var optionPosition = transform.localPosition;
            transform.localPosition = new Vector3(optionPosition.x, startOffset, optionPosition.z);
        }

        foreach (var group in CustomOptionsManager.CustomGroups.Where(group => group.AdvancedRole == null))
        {
            if (group.Header == null) continue;
            group.Header.SetActive(!group.Hidden());

            if (!group.Hidden()) startOffset -= 0.5f;
            var position = group.Header.transform.localPosition;
            group.Header.transform.localPosition = new Vector3(position.x, startOffset, position.z);

            foreach (var option in group.Options)
            {
                if (!option.OptionBehaviour) continue;

                option.OptionBehaviour.enabled = !group.Hidden() && !option.Hidden();
                option.OptionBehaviour.gameObject.SetActive(!group.Hidden() && !option.Hidden());

                if (!group.Hidden() && !option.Hidden()) startOffset -= 0.5f;
                var transform = option.OptionBehaviour.transform;
                var optionPosition = transform.localPosition;
                transform.localPosition = new Vector3(optionPosition.x, startOffset, optionPosition.z);
            }
        }
    }
}