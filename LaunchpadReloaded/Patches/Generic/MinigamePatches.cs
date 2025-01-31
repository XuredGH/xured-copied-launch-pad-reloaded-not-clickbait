using HarmonyLib;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch]
public static class MinigameBeginPatch
{
    [HarmonyPatch(typeof(Minigame), nameof(Minigame.Begin))]
    public static void Prefix(Minigame __instance)
    {
        // this somehow fixes the journal minigame?? credits to submerged
        __instance.logger ??= new Logger("Minigame", Logger.Level.Info, Logger.Category.Gameplay);
    }
}