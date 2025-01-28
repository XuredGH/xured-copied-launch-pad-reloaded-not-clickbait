using HarmonyLib;
using LaunchpadReloaded.Networking;
using MiraAPI.Networking;
using System.Linq;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch]
public static class KillAnimationPatch
{
    public static void MakeDeathData(PlayerControl source, PlayerControl target)
    {
        var suspects = PlayerControl.AllPlayerControls.ToArray()
            .Where(pc => pc != target && !pc.Data.IsDead)
            .Take(5)
            .Select(pc => pc.PlayerId)
            .ToArray();

        target.RpcDeathData(source, suspects);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.CoPerformKill))]
    public static void OnDeathPostfix([HarmonyArgument(0)] PlayerControl source, [HarmonyArgument(1)] PlayerControl target) => MakeDeathData(source, target);

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CustomMurderRpc), nameof(CustomMurderRpc.CoPerformCustomKill))]
    public static void OnDeathMiraPostfix([HarmonyArgument(1)] PlayerControl source, [HarmonyArgument(2)] PlayerControl target) => MakeDeathData(source, target);
}