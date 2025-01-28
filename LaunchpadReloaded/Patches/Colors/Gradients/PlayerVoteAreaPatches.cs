using HarmonyLib;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Colors.Gradients;

[HarmonyPatch(typeof(PlayerVoteArea))]
public static class PlayerVoteAreaPatches
{
    [HarmonyPrefix, HarmonyPatch(nameof(PlayerVoteArea.SetCosmetics))]
    public static void SetCosmeticsPrefix(PlayerVoteArea __instance, [HarmonyArgument(0)] NetworkedPlayerInfo playerInfo)
    {
        __instance.PlayerIcon.gameObject.SetGradientData(playerInfo.PlayerId);
    }
}