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
        if (DragManager.DraggingPlayers.TryGetValue(PlayerControl.LocalPlayer.PlayerId, out var bodyId))
        {
            var bodyById = DragManager.GetBodyById(bodyId);
            bodyById.transform.position = Vector3.Lerp(bodyById.transform.position, __instance.transform.position, 5f * Time.deltaTime);
        }
    }
}