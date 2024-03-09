using HarmonyLib;
using LaunchpadReloaded.Components;
using UnityEngine;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(PlayerMaterial),"SetColors", typeof(int), typeof(Renderer))]
public static class PlayerMaterialPatch
{
    public static void Postfix([HarmonyArgument(0)]int colorId, [HarmonyArgument(1)] Renderer renderer)
    {
        if (renderer.GetComponentInParent<HatParent>())
        {
            return;
        }
        
        if (!renderer.gameObject.GetComponent<GradientColorComponent>())
        {
            renderer.gameObject.AddComponent<GradientColorComponent>();
        }
        
        renderer.gameObject.GetComponent<GradientColorComponent>().SetColor(colorId);
        
    }
}