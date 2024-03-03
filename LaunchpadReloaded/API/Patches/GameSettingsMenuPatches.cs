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
        var numberOpt = CustomGameOptionsManager.NumberOptionPrefab ??= __instance.RegularGameSettings.GetComponentInChildren<NumberOption>();
        var toggleOpt = CustomGameOptionsManager.ToggleOptionPrefab ??= Object.FindObjectOfType<ToggleOption>(); 
        var stringOpt = CustomGameOptionsManager.StringOptionPrefab ??= __instance.RegularGameSettings.GetComponentInChildren<StringOption>(); 
        
        if (!numberOpt || !toggleOpt || !stringOpt)
        {
            Debug.LogError("OPTION PREFABS MISSING");
            return;
        }

        foreach (var customToggleOption in CustomGameOptionsManager.CustomToggleOptions)
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
        
        foreach (var customNumberOption in CustomGameOptionsManager.CustomNumberOptions)
        {
            if (customNumberOption.OptionBehaviour && __instance.AllItems.Contains(customNumberOption.OptionBehaviour.transform)) continue;
            
            var newOpt = Object.Instantiate(numberOpt, numberOpt.transform.parent);
            newOpt.name = customNumberOption.Title;
            newOpt.Title = customNumberOption.StringName;
            newOpt.Value = customNumberOption.Value;    
            newOpt.Increment = customNumberOption.Increment;
            newOpt.SuffixType = customNumberOption.SuffixType;
            newOpt.ValidRange = new FloatRange(customNumberOption.MinValue, customNumberOption.MaxValue);
            newOpt.OnEnable();
            customNumberOption.OptionBehaviour = newOpt;

            __instance.AllItems = __instance.AllItems.AddItem(newOpt.transform).ToArray();
        }
    
    }
    
}