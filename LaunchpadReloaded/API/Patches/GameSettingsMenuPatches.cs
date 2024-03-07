using System.Linq;
using HarmonyLib;
using Il2CppSystem;
using LaunchpadReloaded.API.GameOptions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(GameSettingMenu),"Start")]
public static class GameSettingsMenuPatches
{
    public static void Prefix(GameSettingMenu __instance)
    {
        var numberOpt = __instance.RegularGameSettings.GetComponentInChildren<NumberOption>();
        var toggleOpt = Object.FindObjectOfType<ToggleOption>(); 
        var stringOpt = __instance.RegularGameSettings.GetComponentInChildren<StringOption>(); 
        
        if (!numberOpt || !toggleOpt || !stringOpt)
        {
            Debug.LogError("OPTION PREFABS MISSING");
            return;
        }

        foreach (var customToggleOption in CustomOptionsManager.CustomToggleOptions)
        {
            if (customToggleOption.AdvancedRole is not null || (customToggleOption.OptionBehaviour && __instance.AllItems.Contains(customToggleOption.OptionBehaviour.transform))) continue;

            var newOpt = Object.Instantiate(toggleOpt, toggleOpt.transform.parent);
            customToggleOption.CreateToggleOption(newOpt);
            
            __instance.AllItems = __instance.AllItems.AddItem(newOpt.transform).ToArray();
        }
        
        foreach (var customNumberOption in CustomOptionsManager.CustomNumberOptions)
        {
            if (customNumberOption.AdvancedRole is not null || (customNumberOption.OptionBehaviour && __instance.AllItems.Contains(customNumberOption.OptionBehaviour.transform))) continue;
            
            var newOpt = Object.Instantiate(numberOpt, numberOpt.transform.parent);
            customNumberOption.CreateNumberOption(newOpt);
            
            __instance.AllItems = __instance.AllItems.AddItem(newOpt.transform).ToArray();
        }
    
    }
    
}