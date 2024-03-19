using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.Roles;
using UnityEngine;

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
    [HarmonyPatch(nameof(IntroCutscene.BeginCrewmate))]
    public static void BeginCrewmate(IntroCutscene __instance)
    {
        if (PlayerControl.LocalPlayer.Data.Role is ICustomRole customRole)
        {
            if (!customRole.IsNeutral) return;

            __instance.BackgroundBar.material.SetColor("_Color", Color.gray);
            __instance.TeamTitle.text = "NEUTRAL";
            __instance.TeamTitle.color = Color.gray;
            __instance.ImpostorText.text = string.Empty;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    public static void GameBeginPatch()
    {
        CustomGameModeManager.ActiveMode.Initialize();
    }
}