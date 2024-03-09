using HarmonyLib;
using LaunchpadReloaded.Components;
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

        var color2 = 0;

        if (GameData.Instance && GameData.Instance.GetComponent<CustomGameData>())
        {
            var customData = GameData.Instance.GetComponent<CustomGameData>();
         
            if (renderer.GetComponentInParent<PlayerControl>())
            {
                var pc = renderer.GetComponentInParent<PlayerControl>();
                color2 = customData.GetCustomInfo(pc.PlayerId).SecondColorId;
                Debug.LogError("player found");
            }

            if (renderer.GetComponent<DeadBody>())
            {
                var db = renderer.GetComponent<DeadBody>();
                color2 = customData.GetCustomInfo(db.ParentId).SecondColorId;
                Debug.LogError("dead body found");
            }

            if (renderer.GetComponent<PetBehaviour>())
            {
                var pb = renderer.GetComponent<PetBehaviour>();
                color2 = customData.GetCustomInfo(pb.TargetPlayer.PlayerId).SecondColorId;
            }
        }
        
        if (!renderer.gameObject.GetComponent<GradientColorComponent>())
        {
            renderer.gameObject.AddComponent<GradientColorComponent>();
        }
        
        renderer.gameObject.GetComponent<GradientColorComponent>().SetColor(colorId, color2);
        
    }
}