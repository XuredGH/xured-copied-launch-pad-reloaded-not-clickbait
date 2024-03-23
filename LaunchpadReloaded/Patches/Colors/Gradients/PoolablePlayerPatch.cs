using HarmonyLib;
using LaunchpadReloaded.API.Utilities;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(PoolablePlayer),nameof(PoolablePlayer.UpdateFromPlayerData))]
[HarmonyPatch(typeof(PoolablePlayer),nameof(PoolablePlayer.UpdateFromEitherPlayerDataOrCache))]
public static class PoolablePlayerPatch
{
    public static void Prefix(PoolablePlayer __instance, [HarmonyArgument(0)] GameData.PlayerInfo playerInfo)
    {
        __instance.gameObject.SetGradientData(playerInfo.PlayerId);
    }
}