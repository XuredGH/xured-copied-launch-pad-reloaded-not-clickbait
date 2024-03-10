using HarmonyLib;
using LaunchpadReloaded.Features;
using UnityEngine;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(CosmeticsLayer),nameof(CosmeticsLayer.SetAsLocalPlayer))]
public static class CosmeticsLayerPatch
{
    public static void Prefix()
    {
        Debug.LogError("sending local gradient");
        GradientColorManager.RpcSetGradient(PlayerControl.LocalPlayer,GradientColorManager.Instance.LocalGradientId);
    }
}