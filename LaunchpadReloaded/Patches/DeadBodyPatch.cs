using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(DeadBody))]
public static class DeadBodyPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(DeadBody.OnClick))]
    public static bool OnClickPatch(DeadBody __instance)
    {
        if(CustomGamemodeManager.ActiveMode.CanReport(__instance))
        {
            return !HackingManager.AnyActiveNodes();
        }
        return false;
    }
}
