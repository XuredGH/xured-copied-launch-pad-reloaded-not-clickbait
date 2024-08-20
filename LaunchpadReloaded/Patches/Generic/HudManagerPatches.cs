using System.Linq;
using HarmonyLib;
using InnerNet;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Utilities;
using System.Text;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Features;
using MiraAPI.GameModes;
using MiraAPI.Hud;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{

    /// <summary>
    /// Add the "hacked" task string if local player is hacked
    /// </summary>
    public static void AddHackedTaskString(HudManager __instance)
    {
        __instance.tasksString.Clear();
        __instance.tasksString.Append(Color.green.ToTextColor());
        __instance.tasksString.Append("You have been hacked!\n");
        __instance.tasksString.Append("You are unable to complete tasks or call meetings.\n");
        __instance.tasksString.Append("Find an active node to reverse the hack!.\n");
        __instance.tasksString.Append($"{HackingManager.Instance.hackedPlayers.Count} players are still hacked.");
        __instance.tasksString.Append("</color>");
        __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString());
    }

    /// <summary>
    /// Generic update method for most of HUD logic in Launchpad
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("Update")]
    public static void UpdatePostfix(HudManager __instance)
    {
        var local = PlayerControl.LocalPlayer;
        if (!local || MeetingHud.Instance)
        {
            if (ZoomButton.IsZoom)
            {
                CustomButtonSingleton<ZoomButton>.Instance.OnEffectEnd();
            }
            
            return;
        }

        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started && !ShipStatus.Instance)
        {
            return;
        }

        CustomGameModeManager.ActiveMode?.HudUpdate(__instance);

        if (HackingManager.Instance)
        {
            if (local.Data.IsHacked() && !local.Data.Role.IsImpostor)
            {
                AddHackedTaskString(__instance);
            }
            else if (HackingManager.Instance.AnyPlayerHacked())
            {
                var newB = new StringBuilder();
                newB.Append(Color.green.ToTextColor());
                newB.Append(local.Data.Role.IsImpostor ?
                    "\n\n The crewmates are hacked! They will not be able to\ncomplete tasks or call meetings until they reverse the hack."
                    : "\n\nYou will still not be able to report bodies or \ncall meetings until all crewmates reverse the hack.");
                newB.Append($"\n{HackingManager.Instance.hackedPlayers.Count} players are still hacked.");
                newB.Append("</color>");
                __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString() + newB);
            }

            if (HackingManager.Instance.AnyPlayerHacked())
            {
                __instance.ReportButton.SetActive(false);
            }
        }
        
        foreach (var player in LaunchpadPlayer.GetAllPlayers().Where(x=>x.Dragging))
        {
            var bodyById = Helpers.GetBodyById(player.dragId);
            bodyById.transform.position = Vector3.Lerp(bodyById.transform.position, player.transform.position, 5f * Time.deltaTime);
        }
    }
}