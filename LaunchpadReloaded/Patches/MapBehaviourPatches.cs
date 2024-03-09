using GameCore;
using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(MapBehaviour))]
public class MapBehaviourPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MapBehaviour.ShowSabotageMap))]
    public static bool ShowSabotagePatch(MapBehaviour __instance)
    {
        bool shouldShow = CustomGamemodeManager.ActiveMode.ShouldShowSabotageMap(__instance);
        if(!shouldShow)
        {
            __instance.ShowNormalMap();
            return false;
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MapBehaviour.Show))]
    public static bool MapBehaviourShowPrefix()
    {
        return !Helpers.ShouldCancelClick();
    }
    
}