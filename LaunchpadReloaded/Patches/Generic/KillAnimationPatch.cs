using System.Linq;
using HarmonyLib;
using LaunchpadReloaded.Networking;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(KillAnimation))]
public static class KillAnimationPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(KillAnimation.CoPerformKill))]
    public static void OnDeathPostfix([HarmonyArgument(0)] PlayerControl source, [HarmonyArgument(1)] PlayerControl target)
    {
        var suspects = PlayerControl.AllPlayerControls.ToArray()
            .Where(pc => pc != target && !pc.Data.IsDead)
            .Take(4)
            .Select(pc => pc.PlayerId)
            .ToArray();
        source.RpcDeathData(target, suspects);
    }
}