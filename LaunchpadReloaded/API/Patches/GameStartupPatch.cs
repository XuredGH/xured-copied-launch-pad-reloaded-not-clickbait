using HarmonyLib;
using LaunchpadReloaded.API.Roles;

namespace LaunchpadReloaded.API.Patches;

// TODO: FIND A BETTER PLACE TO RUN STARTUP THINGS
[HarmonyPatch(typeof(MainMenuManager))]
public static class GameStartupPatch
{
    private static bool _runOnce;
    
    [HarmonyPostfix]
    [HarmonyPatch(nameof(MainMenuManager.Start))]
    public static void Postfix()
    {
        if (_runOnce) return;
        _runOnce = true;
        CustomRoleManager.Register();
    }
}