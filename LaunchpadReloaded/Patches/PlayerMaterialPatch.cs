using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using UnityEngine;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(PlayerMaterial),"SetColors", typeof(int), typeof(Renderer))]
public static class PlayerMaterialPatch
{
    public static void Postfix([HarmonyArgument(0)]int colorId, [HarmonyArgument(1)] Renderer renderer)
    {
        if (renderer.GetComponentInParent<HatParent>() && !renderer.GetComponentInParent<CosmeticsLayer>())
        {
            return;
        }

        var color2 = GradientColorManager.Instance.LocalGradientId;

        if (GameData.Instance)
        {
            if (renderer.GetComponentInParent<PlayerControl>())
            {
                var pc = renderer.GetComponentInParent<PlayerControl>();
                color2 = GradientColorManager.Instance.Gradients[pc.PlayerId];
            }

            if (renderer.GetComponent<DeadBody>())
            {
                var db = renderer.GetComponent<DeadBody>();
                color2 = GradientColorManager.Instance.Gradients[db.ParentId];
                Debug.LogError("dead body found");
            }
        }
        
        var gradColor = renderer.GetComponent<GradientColorComponent>();
        if (!gradColor)
        {
            gradColor = renderer.gameObject.AddComponent<GradientColorComponent>();
        }
        
        gradColor.SetColor(colorId, color2);
        
    }
}