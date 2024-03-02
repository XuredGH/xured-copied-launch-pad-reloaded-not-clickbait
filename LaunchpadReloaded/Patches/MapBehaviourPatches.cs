using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(MapBehaviour))]
public class MapBehaviourPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MapBehaviour.Show))]
    public static bool MapBehaviourShowPrefix()
    {
        return !Helpers.ShouldCancelClick();
    }
    
}