using System;
using HarmonyLib;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Networking;
using System.Linq;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch]
public static class KillAnimationPatch
{
    public static void MakeDeathData(PlayerControl source, PlayerControl target)
    {
        var suspects = PlayerControl.AllPlayerControls.ToArray()
            .Where(pc => pc != target && pc != source && !pc.Data.IsDead && pc.Data.Role is not DetectiveRole)
            .Take((int)OptionGroupSingleton<DetectiveOptions>.Instance.SuspectCount)
            .Append(source);

        var deathData = new DeathData(DateTime.UtcNow, source, suspects);
        target.GetModifierComponent()!.AddModifier(deathData);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.CoPerformKill))]
    public static void OnDeathPostfix([HarmonyArgument(0)] PlayerControl source, [HarmonyArgument(1)] PlayerControl target) => MakeDeathData(source, target);

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CustomMurderRpc), nameof(CustomMurderRpc.CoPerformCustomKill))]
    public static void OnDeathMiraPostfix([HarmonyArgument(1)] PlayerControl source, [HarmonyArgument(2)] PlayerControl target) => MakeDeathData(source, target);
}