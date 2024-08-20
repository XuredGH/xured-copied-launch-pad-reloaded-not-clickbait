using HarmonyLib;
using LaunchpadReloaded.Features.Managers;
using MiraAPI.GameModes;

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
        if (CustomGameModeManager.ActiveMode != null && CustomGameModeManager.ActiveMode.CanReport(__instance))
        {
            return !HackingManager.Instance.AnyPlayerHacked();
        }
        return false;
    }
}
