using System.Runtime.CompilerServices;
using HarmonyLib;

namespace LaunchpadReloaded.Patches.Reverse;

[HarmonyPatch]
public static class MinigameStubs
{
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Minigame), nameof(Minigame.Close), [])]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Close(Minigame instance)
    {
        // nothing needed
    }
}