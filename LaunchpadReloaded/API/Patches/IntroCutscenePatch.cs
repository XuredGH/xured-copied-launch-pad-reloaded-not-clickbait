using GameCore;
using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(IntroCutscene))]
public static class IntroCutscenePatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(IntroCutscene.BeginImpostor))]
    public static void BeginImpostorPatch(IntroCutscene __instance)
    {
        if (CustomGamemodeManager.ActiveMode.ShowCustomRoleScreen())
        {
            CustomGamemode mode = CustomGamemodeManager.ActiveMode;
            __instance.TeamTitle.text = $"<size=70%>{mode.Name}</size>\n<size=20%>{mode.Description}</size>";
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    public static void GameBeginPatch()
    {
        CustomGamemodeManager.ActiveMode.Initialize();
    }
}