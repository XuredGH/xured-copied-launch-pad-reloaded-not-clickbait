using HarmonyLib;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(MapBehaviour))]
public class MapBehaviourPatches
{
    /// <summary>
    /// Only show map if click is not cancelled
    /// </summary>
    [HarmonyPrefix, HarmonyPatch(nameof(MapBehaviour.Show))]
    public static bool MapBehaviourShowPrefix()
    {
        return !Helpers.ShouldCancelClick();
    }
}