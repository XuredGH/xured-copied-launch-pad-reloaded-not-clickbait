using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.Features.Managers;

namespace LaunchpadReloaded.Patches.Roles.Hacker;

/// <summary>
/// Disable reporting bodies if hacked
/// </summary>
[HarmonyPatch(typeof(DeadBody))]
public static class DeadBodyPatch
{
    [HarmonyPrefix, HarmonyPatch("OnClick")]
    public static bool OnClickPatch(DeadBody __instance)
    {
        if (CustomGameModeManager.ActiveMode.CanReport(__instance))
        {
            return !HackingManager.Instance.AnyPlayerHacked();
        }
        return false;
    }
}
