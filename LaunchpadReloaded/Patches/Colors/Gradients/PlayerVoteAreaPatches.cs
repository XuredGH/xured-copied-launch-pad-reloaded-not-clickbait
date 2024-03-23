using HarmonyLib;
using LaunchpadReloaded.API.Utilities;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(PlayerVoteArea))]
public static class PlayerVoteAreaPatches
{
    [HarmonyPrefix, HarmonyPatch(nameof(PlayerVoteArea.SetCosmetics))]
    public static void SetCosmeticsPrefix(PlayerVoteArea __instance, [HarmonyArgument(0)] GameData.PlayerInfo playerInfo)
    {
        __instance.PlayerIcon.gameObject.SetGradientData(playerInfo.PlayerId);
    }
}