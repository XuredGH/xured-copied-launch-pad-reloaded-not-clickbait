using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking;
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

        if(HackingManager.HackedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId))
        {
            __instance.tasksString.Clear();
            __instance.tasksString.Append(UnityEngine.Color.green.ToTextColor());
            __instance.tasksString.Append("You have been hacked!\n");
            __instance.tasksString.Append("You are unable to complete tasks or call meetings.\n");
            __instance.tasksString.Append("Find an active node to reverse the hack!.\n");
            __instance.tasksString.Append("</color>");

            __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString());
        }

        if (HackingManager.AnyActiveNodes()) __instance.ReportButton.SetDisabled();

        foreach (var (player, bodyId) in DragManager.DraggingPlayers)
        {
            var bodyById = DeadBodyManager.GetBodyById(bodyId);
            bodyById.transform.position = Vector3.Lerp(bodyById.transform.position, GameData.Instance.GetPlayerById(player).Object.transform.position, 5f * Time.deltaTime);
        }
    }
    
}