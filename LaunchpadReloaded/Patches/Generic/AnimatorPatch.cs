using HarmonyLib;
using LaunchpadReloaded.Roles.Impostor;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch]
public static class AnimatorPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Animator), nameof(Animator.Update))]
    public static void SetSpeedPatch(Animator __instance)
    {
        if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data.Role is HitmanRole hitman && hitman.InDeadlockMode)
        {
            float globalSpeedMultiplier = 0.5f;
            __instance.speed *= globalSpeedMultiplier;
        }
    }
}