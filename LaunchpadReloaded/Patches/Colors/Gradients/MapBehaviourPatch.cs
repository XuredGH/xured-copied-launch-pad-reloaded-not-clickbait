using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Colors.Gradients;

[HarmonyPatch(typeof(MapBehaviour),nameof(MapBehaviour.Awake))]
public static class MapBehaviourPatch
{
    public static void Postfix(MapBehaviour __instance)
    {
        __instance.HerePoint.material = LaunchpadAssets.GradientMaterial.LoadAsset();
        
        __instance.HerePoint.gameObject.AddComponent<GradientColorComponent>().SetColor(PlayerControl.LocalPlayer.Data.DefaultOutfit.ColorId, PlayerControl.LocalPlayer.GetComponent<PlayerGradientData>().GradientColor);

        var mat = __instance.HerePoint.material;
        
        mat.SetFloat(ShaderID.GradientBlend, 5);
        mat.SetFloat(ShaderID.GradientOffset, .35f);
        
    }
}