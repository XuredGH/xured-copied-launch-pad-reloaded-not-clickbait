using HarmonyLib;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(AmongUsClient),nameof(AmongUsClient.CreatePlayer))]
public static class AmongUsClientPatch
{
    public static void Postfix(AmongUsClient __instance)
    {
        foreach (var player in GameData.Instance.AllPlayers)
        {
            GradientColorManager.RpcRequestGradient(player.Object);
        }
    }
}