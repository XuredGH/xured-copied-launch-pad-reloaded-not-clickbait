using System.Linq;
using HarmonyLib;
using Il2CppInterop.Runtime;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using UnityEngine;

namespace LaunchpadReloaded.Patches.GradientColor;

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

        if (PlayerCustomizationMenu.Instance && PlayerTabPatches.SelectGradient)
        {
            color2 = PlayerCustomizationMenu.Instance.GetComponentInChildren<PlayerTab>().currentColor;
        }
        
        if (GameData.Instance)
        {
            if (renderer.GetComponentInParent<PlayerControl>())
            {
                var pc = renderer.GetComponentInParent<PlayerControl>();
                if (GradientColorManager.Instance.Gradients.TryGetValue(pc.PlayerId, out var gradient))
                {
                    color2 = gradient;
                }
            }

            if (renderer.GetComponent<DeadBody>())
            {
                var db = renderer.GetComponent<DeadBody>();
                color2 = GradientColorManager.Instance.Gradients[db.ParentId];
                Debug.LogError("dead body found");
            }

            if (renderer.GetComponent<PetBehaviour>())
            {
                var pet = renderer.GetComponent<PetBehaviour>();
                if (pet.TargetPlayer)
                {
                    color2 = GradientColorManager.Instance.Gradients[pet.TargetPlayer.PlayerId];
                }
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