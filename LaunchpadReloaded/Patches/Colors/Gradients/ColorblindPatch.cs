using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities;

namespace LaunchpadReloaded.Patches.Colors.Gradients;

[HarmonyPatch(typeof(CosmeticsLayer))]
public static class ColorblindPatch
{
    [HarmonyPrefix, HarmonyPatch(nameof(CosmeticsLayer.GetColorBlindText))]
    public static bool CosmeticsLayerPatch(CosmeticsLayer __instance, ref string __result)
    {
        if (!__instance.TryGetComponent(out PlayerGradientData comp) &&
            !__instance.transform.parent.TryGetComponent(out comp))
        {
            Logger<LaunchpadReloadedPlugin>.Error(__instance.transform.parent.name);
            return true;
        }

        GameData.PlayerInfo plr = GameData.Instance.GetPlayerById(comp.playerId);

        if (plr.IsHacked() || (HackingManager.Instance && HackingManager.Instance.AnyActiveNodes() && plr.Role is HackerRole))
        {
            __result = "???";
            return false;
        }

        if (!comp.GradientEnabled)
        {
            return true;
        }

        var defaultColor = Helpers.FirstLetterToUpper(Palette.GetColorName(__instance.ColorId).ToLower());
        var gradientColor = Helpers.FirstLetterToUpper(Palette.GetColorName(comp.GradientColor).ToLower());

        if (defaultColor == gradientColor || gradientColor == "???")
        {
            __result = defaultColor;
            return false;
        }

        if (!__instance.GetComponentInParent<PlayerVoteArea>() &&
            !__instance.GetComponentInParent<PlayerControl>() &&
            !__instance.GetComponentInParent<ShapeshifterPanel>())
        {
            __result = $"{gradientColor}\n{defaultColor}";
        }
        else
        {
            __result = $"{gradientColor}-{defaultColor}";
        }

        return false;
    }
}