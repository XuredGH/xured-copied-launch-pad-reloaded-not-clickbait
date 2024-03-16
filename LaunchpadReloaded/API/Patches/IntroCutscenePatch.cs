using HarmonyLib;
using LaunchpadReloaded.API.GameModes;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(IntroCutscene))]
public static class IntroCutscenePatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(IntroCutscene.BeginImpostor))]
    public static void BeginImpostorPatch(IntroCutscene __instance)
    {
        if (CustomGameModeManager.ActiveMode.ShowCustomRoleScreen())
        {
            var mode = CustomGameModeManager.ActiveMode;
            __instance.TeamTitle.text = $"<size=70%>{mode.Name}</size>\n<size=20%>{mode.Description}</size>";
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    public static void GameBeginPatch()
    {
        CustomGameModeManager.ActiveMode.Initialize();
    }
}