using HarmonyLib;
using LaunchpadReloaded.API.Utilities;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(PlayerVoteArea),nameof(PlayerVoteArea.SetCosmetics))]
public static class PlayerVoteAreaPatch
{
    public static void Prefix(PlayerVoteArea __instance, [HarmonyArgument(0)] GameData.PlayerInfo playerInfo)
    {
        __instance.PlayerIcon.gameObject.SetGradientData(playerInfo.PlayerId);
    }
}