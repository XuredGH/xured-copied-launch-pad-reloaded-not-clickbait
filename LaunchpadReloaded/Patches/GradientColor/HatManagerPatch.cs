using HarmonyLib;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(HatManager),nameof(HatManager.Initialize))]
public static class HatManagerPatch
{
    public static void Postfix(HatManager __instance)
    {
        var mat1 = __instance.PlayerMaterial = LaunchpadAssets.GradientMaterial.LoadAsset();
        var mat2 = __instance.MaskedPlayerMaterial = LaunchpadAssets.MaskedGradientMaterial.LoadAsset();
        
        mat1.SetFloat(ShaderID.GradientBlend,1);
        mat1.SetVector(ShaderID.GradientOffset,new Vector4(0,.25f,1,1));
        
        mat2.SetFloat(ShaderID.GradientBlend,1);
        mat2.SetVector(ShaderID.GradientOffset,new Vector4(0,.25f,1,1));
        
    }
}