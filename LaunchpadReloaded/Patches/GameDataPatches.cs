using HarmonyLib;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(GameData))]
public static class GameDataPatches
{
    [HarmonyPrefix]
    [HarmonyPatch("AddPlayer")]
    public static void AddPlayerPrefix([HarmonyArgument(0)] PlayerControl pc)
    {
        GradientColorManager.Instance.Gradients[pc.PlayerId] = 0;
    }
}