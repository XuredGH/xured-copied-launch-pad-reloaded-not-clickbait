using HarmonyLib;
using LaunchpadReloaded.API.Patches;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using Reactor.Utilities.Extensions;
using System.Text;
using UnityEngine;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("Update")]
    public static void UpdatePostfix(HudManager __instance)
    {
        if (!PlayerControl.LocalPlayer || MeetingHud.Instance)
        {
            return;
        }

        if (PlayerControl.LocalPlayer.Data.IsHacked())
        {
            __instance.tasksString.Clear();
            __instance.tasksString.Append(Color.green.ToTextColor());
            __instance.tasksString.Append("You have been hacked!\n");
            __instance.tasksString.Append("You are unable to complete tasks or call meetings.\n");
            __instance.tasksString.Append("Find an active node to reverse the hack!.\n");
            __instance.tasksString.Append($"{HackingManager.Instance.hackedPlayers.Count} players are still hacked.");
            __instance.tasksString.Append("</color>");
            __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString());

            if (HudManagerPatchesApi._roleTab != null)
            {
                HudManagerPatchesApi._roleTab.gameObject.Destroy();
            }
        }
        else if (HackingManager.Instance && HackingManager.Instance.AnyActiveNodes())
        {
            var newB = new StringBuilder();
            newB.Append(Color.green.ToTextColor());
            newB.Append(PlayerControl.LocalPlayer.Data.Role is HackerRole ?
                "\n\nYou have hacked the crewmates! They will not be able to\ncomplete tasks or call meetings until they reverse the hack."
                : "\n\nYou will still not be able to report bodies or \ncall meetings until all crewmates reverse the hack.");
            newB.Append($"\n{HackingManager.Instance.hackedPlayers.Count} players are still hacked.");
            newB.Append("</color>");
            __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString() + newB);
        }

        if (DragManager.Instance is null || HackingManager.Instance is null) return;

        if (HackingManager.Instance.AnyActiveNodes())
        {
            __instance.ReportButton.SetDisabled();
        }

        foreach (var (player, bodyId) in DragManager.Instance.DraggingPlayers)
        {
            var bodyById = DeadBodyManager.GetBodyById(bodyId);
            bodyById.transform.position = Vector3.Lerp(bodyById.transform.position, GameData.Instance.GetPlayerById(player).Object.transform.position, 5f * Time.deltaTime);
        }
    }

}