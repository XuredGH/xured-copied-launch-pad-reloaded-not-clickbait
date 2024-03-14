﻿using HarmonyLib;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(AmongUsClient),nameof(AmongUsClient.OnPlayerJoined))]
public static class AmongUsClientPatch
{
    public static void Postfix()
    {
        GradientManager.RpcSetGradient(PlayerControl.LocalPlayer, GradientManager.LocalGradientId);
    }
}