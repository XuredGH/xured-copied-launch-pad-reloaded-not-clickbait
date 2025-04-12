using HarmonyLib;
using LaunchpadReloaded.Modifiers.Fun;
using MiraAPI.Modifiers;

namespace LaunchpadReloaded.Patches.Modifiers;

[HarmonyPatch(typeof(MedScanMinigame))]
public static class MedscanPatch
{
    [HarmonyPatch(nameof(MedScanMinigame.Begin))]
    [HarmonyPostfix]
    public static void OverrideSizePatch(MedScanMinigame __instance)
    {
        if (PlayerControl.LocalPlayer.HasModifier<HumongousModifier>())
        {
            __instance.completeString = __instance.completeString.Replace("3' 6\"", "7' 9\"").Replace("92lb", "734lb");
        }

        if (PlayerControl.LocalPlayer.HasModifier<ShortModifier>())
        {
            __instance.completeString = __instance.completeString.Replace("3' 6\"", "0' 0.1\"").Replace("92lb", "1lb");
        }

        if (PlayerControl.LocalPlayer.HasModifier<SmolModifier>())
        {
            __instance.completeString = __instance.completeString.Replace("3' 6\"", "1' 8\"").Replace("92lb", "46lb");
        }

        if (PlayerControl.LocalPlayer.HasModifier<BabyModifier>())
        {
            __instance.completeString = __instance.completeString.Replace("3' 6\"", "0' 1\"").Replace("92lb", "1lb");
        }
    }
}