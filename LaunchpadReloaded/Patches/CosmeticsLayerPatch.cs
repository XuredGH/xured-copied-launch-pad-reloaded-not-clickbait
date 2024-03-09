using HarmonyLib;
using LaunchpadReloaded.Components;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(CosmeticsLayer),"SetColor")]
public static class CosmeticsLayerPatch
{
    public static void Postfix(CosmeticsLayer __instance)
    {
        foreach (var bodySprite in __instance.bodySprites)
        {
            var grad = bodySprite.BodySprite.GetComponent<GradientColorComponent>();
            if (!grad)
            {
                grad = bodySprite.BodySprite.gameObject.AddComponent<GradientColorComponent>();
                grad.playerId = __instance.GetComponentInParent<PlayerControl>().PlayerId;
            }
            grad.Initialize();
        }
    }
}