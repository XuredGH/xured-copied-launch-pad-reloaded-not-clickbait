using HarmonyLib;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Roles.Scientist;

[HarmonyPatch(typeof(ScientistRole))]
public static class ScientistPatches
{
    [HarmonyPatch(nameof(ScientistRole.RefreshAbilityButton))]
    public static bool Prefix()
    {
        if (PlayerControl.LocalPlayer.Data.IsHacked())
        {
            DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
            return false;
        }

        return true;
    }
}