using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(DeadBody))]
public static class DeadBodyPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(DeadBody.OnClick))]
    public static bool OnClickPatch(DeadBody __instance)
    {
        if (CustomGameModeManager.ActiveMode.CanReport(__instance))
        {
            return !HackingManager.Instance.AnyActiveNodes();
        }
        return false;
    }
}
