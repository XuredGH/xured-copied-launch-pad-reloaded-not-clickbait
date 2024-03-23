using HarmonyLib;
using LaunchpadReloaded.Features.Managers;

namespace LaunchpadReloaded.Patches.Colors.Gradients;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
public static class AmongUsClientPatch
{
    public static void Postfix()
    {
        GradientManager.RpcSetGradient(PlayerControl.LocalPlayer, GradientManager.LocalGradientId);
    }
}