using HarmonyLib;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Colors.Gradients;

[HarmonyPatch(typeof(PoolablePlayer))]
public static class PoolablePlayerPatches
{
    [HarmonyPatch(nameof(PoolablePlayer.UpdateFromPlayerData))]
    [HarmonyPatch(nameof(PoolablePlayer.UpdateFromEitherPlayerDataOrCache))]
    public static void UpdateFromPlayerPrefix(PoolablePlayer __instance, [HarmonyArgument(0)] GameData.PlayerInfo playerInfo)
    {
        __instance.gameObject.SetGradientData(playerInfo.PlayerId);
        var mat = __instance.cosmetics.currentBodySprite.BodySprite.material;
        
        mat.SetFloat(ShaderID.GradientBlend, 3f);
        mat.SetFloat(ShaderID.GradientOffset, .4f);
    }
}