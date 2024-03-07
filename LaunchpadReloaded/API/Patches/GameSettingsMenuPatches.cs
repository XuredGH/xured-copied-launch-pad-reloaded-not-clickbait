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
            if (customToggleOption.OptionBehaviour && __instance.AllItems.Contains(customToggleOption.OptionBehaviour.transform)) continue;

            var newOpt = Object.Instantiate(toggleOpt, toggleOpt.transform.parent);
            newOpt.name = customToggleOption.Title;
            newOpt.Title = customToggleOption.StringName;
            newOpt.CheckMark.enabled = customToggleOption.Value;
            newOpt.OnEnable();

            customToggleOption.OptionBehaviour = newOpt;
            
            __instance.AllItems = __instance.AllItems.AddItem(newOpt.transform).ToArray();
        }
        
        foreach (var customNumberOption in CustomOptionsManager.CustomNumberOptions)
        {
            if (customNumberOption.OptionBehaviour && __instance.AllItems.Contains(customNumberOption.OptionBehaviour.transform)) continue;
            
            var newOpt = Object.Instantiate(numberOpt, numberOpt.transform.parent);
            newOpt.name = customNumberOption.Title;
            newOpt.Title = customNumberOption.StringName;
            newOpt.Value = customNumberOption.Value;    
            newOpt.Increment = customNumberOption.Increment;
            newOpt.SuffixType = customNumberOption.SuffixType;
            newOpt.FormatString = customNumberOption.NumberFormat;
            newOpt.ValidRange = new FloatRange(customNumberOption.MinValue, customNumberOption.MaxValue);
            newOpt.OnEnable();
            customNumberOption.OptionBehaviour = newOpt;

            __instance.AllItems = __instance.AllItems.AddItem(newOpt.transform).ToArray();
        }
    
    }
    
}