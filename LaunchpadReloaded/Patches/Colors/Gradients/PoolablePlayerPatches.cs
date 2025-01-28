using HarmonyLib;
using LaunchpadReloaded.Utilities;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Patches.Colors.Gradients;

[HarmonyPatch(typeof(PoolablePlayer))]
public static class PoolablePlayerPatches
{
    [HarmonyPatch(nameof(PoolablePlayer.UpdateFromPlayerData))]
    [HarmonyPatch(nameof(PoolablePlayer.UpdateFromEitherPlayerDataOrCache))]
    public static void Prefix(PoolablePlayer __instance, [HarmonyArgument(0)] NetworkedPlayerInfo playerInfo)
    {
        __instance.gameObject.SetGradientData(playerInfo.PlayerId);
        var mat = __instance.cosmetics.currentBodySprite.BodySprite.material;
        
        mat.SetFloat(ShaderID.Get("_GradientBlend"), 25f);
        mat.SetFloat(ShaderID.Get("_GradientOffset"), .4f);
    }
}