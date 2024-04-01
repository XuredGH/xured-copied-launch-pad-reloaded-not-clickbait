using HarmonyLib;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking;

namespace LaunchpadReloaded.Patches.Colors.Gradients;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
public static class AmongUsClientPatch
{
    public static void Postfix()
    {
        PlayerControl.LocalPlayer.RpcSetGradient(GradientManager.LocalGradientId);
    }
}