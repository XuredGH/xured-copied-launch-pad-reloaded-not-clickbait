using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.Roles;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(IntroCutscene))]
public static class IntroCutscenePatch
{
    /// <summary>
    /// Used for setting the team text is gamemodes override it (mainly for Battle Royale currently)
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("BeginImpostor")]
    public static void BeginImpostorPatch(IntroCutscene __instance)
    {
        if (CustomGameModeManager.ActiveMode.ShowCustomRoleScreen())
        {
            var mode = CustomGameModeManager.ActiveMode;
            __instance.TeamTitle.text = $"<size=70%>{mode.Name}</size>\n<size=20%>{mode.Description}</size>";
        }
    }

    /// <summary>
    /// Outcasts are technically crewmate-based roles, so we are patching the crewmate intro cutscene 
    /// and replacing it with an "outcast" cutscene
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("BeginCrewmate")]
    public static bool BeginCrewmatePatch(IntroCutscene __instance)
    {
        if (PlayerControl.LocalPlayer.Data.Role is ICustomRole customRole)
        {
            if (!customRole.IsOutcast) return true;

            Vector3 position = __instance.BackgroundBar.transform.position;
            position.y -= 0.25f;
            __instance.BackgroundBar.transform.position = position;

            __instance.BackgroundBar.material.SetColor("_Color", Color.gray);
            __instance.TeamTitle.text = "OUTCAST";
            __instance.impostorScale = 1f;
            __instance.ImpostorText.text = "You are an Outcast. You do not have a team.";
            __instance.TeamTitle.color = Color.gray;

            __instance.ourCrewmate = __instance.CreatePlayer(0, Mathf.CeilToInt(7.5f), PlayerControl.LocalPlayer.Data, false);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Initialize current gamemode when game starts
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("OnDestroy")]
    public static void GameBeginPatch()
    {
        CustomGameModeManager.ActiveMode.Initialize();
    }
}