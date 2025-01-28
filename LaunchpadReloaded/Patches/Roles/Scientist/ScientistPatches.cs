using HarmonyLib;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Roles.Scientist;

[HarmonyPatch(typeof(ScientistRole))]
public static class ScientistPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ScientistRole.RefreshAbilityButton))]
    public static bool RefreshButtonPatch()
    {
        if (PlayerControl.LocalPlayer.Data.IsHacked())
        {
            DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
            return false;
        }

        return true;
    }
}