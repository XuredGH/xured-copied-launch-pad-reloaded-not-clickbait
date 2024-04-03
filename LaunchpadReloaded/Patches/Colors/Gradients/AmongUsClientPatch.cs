using HarmonyLib;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking;

namespace LaunchpadReloaded.Patches.Colors.Gradients;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
public static class AmongUsClientPatch
{
    public static void Postfix()
    {
        if (!AmongUsClient.Instance.AmHost)
        {
            return;
        }
        
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (GradientManager.TryGetColor(player.PlayerId, out var color))
            {
                GameData.Instance.CustomSetColor(player, (byte)player.Data.DefaultOutfit.ColorId, color);
                
            }
        }
    }
}