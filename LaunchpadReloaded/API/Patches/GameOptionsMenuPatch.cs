using HarmonyLib;
using Il2CppSystem;
using LaunchpadReloaded.API.GameOptions;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(GameOptionsMenu))]
public static class GameOptionsMenuPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("Start")]
    public static void StartPostfix(GameOptionsMenu __instance)
    {
        foreach (var customOption in CustomGameOptionsManager.CustomOptions)
        {
            customOption.OptionBehaviour.OnValueChanged = (Action<OptionBehaviour>)customOption.ValueChanged;
        }
    }
}