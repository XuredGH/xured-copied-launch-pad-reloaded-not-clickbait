using System.Collections.Generic;
using System.Reflection;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch]
public static class CrowdedModPatch
{
    public const string CrowdedId = "xyz.crowdedmods.crowdedmod";

    public static IEnumerable<MethodBase> TargetMethods()
    {
        var crowdedLoaded = IL2CPPChainloader.Instance.Plugins.TryGetValue(CrowdedId, out var plugin);

        if (crowdedLoaded && plugin?.Instance.GetType().Assembly.GetType("CrowdedMod.Patches.GenericPatches") is { } genericPatches)
        {
            yield return AccessTools.PropertyGetter(genericPatches, "ShouldDisableColorPatch");
        }
    }

    // ReSharper disable once InconsistentNaming
    public static void Postfix(ref bool __result)
    {
        __result = true;

    }
}