using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using UnityEngine;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(PlayerMaterial), "SetColors", typeof(int), typeof(Renderer))]
public static class PlayerMaterialPatch
{
    public static void Postfix([HarmonyArgument(0)] int colorId, [HarmonyArgument(1)] Renderer renderer)
    {
        if (renderer.GetComponentInParent<HatParent>() && !renderer.GetComponentInParent<CosmeticsLayer>())
        {
            return;
        }

        var color2 = GradientManager.LocalGradientId;

        if (PlayerCustomizationMenu.Instance && PlayerTabPatches.SelectGradient)
        {
            color2 = PlayerCustomizationMenu.Instance.GetComponentInChildren<PlayerTab>().currentColor;
        }

        if (GameData.Instance)
        {
            byte id = 255;
            if (renderer.GetComponentInParent<PlayerControl>())
            {
                id = renderer.GetComponentInParent<PlayerControl>().PlayerId;
            }

            if (renderer.GetComponent<DeadBody>())
            {
                id = renderer.GetComponent<DeadBody>().ParentId;
            }

            if (renderer.GetComponent<PetBehaviour>())
            {
                var pet = renderer.GetComponent<PetBehaviour>();
                if (pet.TargetPlayer)
                {
                    id = pet.TargetPlayer.PlayerId;
                }
            }

            if (id != 255 && GradientManager.TryGetColor(id, out var color))
            {
                color2 = color;
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