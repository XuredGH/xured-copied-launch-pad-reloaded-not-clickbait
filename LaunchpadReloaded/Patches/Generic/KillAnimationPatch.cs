using HarmonyLib;
using LaunchpadReloaded.Utilities;
using MiraAPI.GameModes;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(KillAnimation))]
public static class KillAnimationPatch
{
    [HarmonyPostfix, HarmonyPatch(nameof(KillAnimation.CoPerformKill))]
    public static void OnDeathPostfix([HarmonyArgument(0)] PlayerControl source, [HarmonyArgument(1)] PlayerControl target)
    {
        CustomGameModeManager.ActiveMode?.OnDeath(target);
        target.GetLpPlayer().OnDeath(source);
    }
}