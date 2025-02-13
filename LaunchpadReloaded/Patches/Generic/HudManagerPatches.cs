using HarmonyLib;
using InnerNet;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using MiraAPI.Hud;
using System.Text;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{
    /// <summary>
    /// Create Notepad button and screen
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(HudManager.Start))]
    public static void StartPostfix(HudManager __instance)
    {
        if (NotepadHud.Instance != null)
        {
            NotepadHud.Instance.Destroy();
        }

        new NotepadHud(__instance);
    }

    /// <summary>
    /// Generic update method for most of HUD logic in Launchpad
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(HudManager.Update))]
    public static void UpdatePostfix(HudManager __instance)
    {
        var local = PlayerControl.LocalPlayer;
        if (!local || MeetingHud.Instance)
        {
            if (CustomButtonSingleton<ZoomButton>.Instance.EffectActive)
            {
                CustomButtonSingleton<ZoomButton>.Instance.OnEffectEnd();
            }

            return;
        }

        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started && !ShipStatus.Instance)
        {
            return;
        }

        if (!(local.Data.IsHacked() && !local.Data.Role.IsImpostor) && HackerUtilities.AnyPlayerHacked())
        {
            var newB = new StringBuilder();
            newB.Append(Color.green.ToTextColor());
            newB.Append(local.Data.Role.IsImpostor ?
                "\n\n The crewmates are hacked! They will not be able to\ncomplete tasks or call meetings until they reverse the hack."
                : "\n\nYou will still not be able to report bodies or \ncall meetings until all crewmates reverse the hack.");
            newB.Append($"\n{HackerUtilities.CountHackedPlayers()} players are still hacked.");
            newB.Append("</color>");
            __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString() + newB);
        }

        if (HackerUtilities.AnyPlayerHacked())
        {
            __instance.ReportButton.SetActive(false);
        }
    }
}