using HarmonyLib;
using LaunchpadReloaded.Features;
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

        foreach (var (player, bodyId) in DragManager.DraggingPlayers)
        {
            var bodyById = DragManager.GetBodyById(bodyId);
            bodyById.transform.position = Vector3.Lerp(bodyById.transform.position, GameData.Instance.GetPlayerById(player).Object.transform.position, 5f * Time.deltaTime);
        }
    }
}