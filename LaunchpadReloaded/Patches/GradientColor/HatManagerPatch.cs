using HarmonyLib;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(HatManager),nameof(HatManager.Initialize))]
public static class HatManagerPatch
{
    private static readonly Vector4 Offset = new(0, .35f, .5f, 1);
    private const float Strength = 125;

    public static void Postfix(HatManager __instance)
    {
        var mat = __instance.PlayerMaterial = __instance.MaskedPlayerMaterial = __instance.MaskedMaterial = 
            __instance.DefaultShader = LaunchpadAssets.GradientMaterial.LoadAsset();
        
        mat.SetFloat(ShaderID.GradientStrength, Strength);
        mat.SetVector(ShaderID.GradientOffset, Offset);
    }
}