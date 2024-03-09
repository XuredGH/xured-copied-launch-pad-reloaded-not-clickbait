using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(PlayerControl))]
public static class PlayerControlPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.IsKillTimerEnabled),MethodType.Getter)]
    public static void GetKillTimerEnabledPostfix(PlayerControl __instance, ref bool __result)
    {
        switch (__instance.Data.Role)
        {
            case JanitorRole:
                __result = __result && !DragManager.IsDragging(__instance.PlayerId);
                break;
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerControl.SetPlayerMaterialColors))]
    public static bool SetPlayerMatPostfix(PlayerControl __instance, [HarmonyArgument(0)] Renderer sprite)
    {
        Debug.LogError($"Setting gradient color for {__instance.Data.PlayerName}");
        if (!sprite.gameObject.GetComponent<GradientColorComponent>())
        {
            var grad = sprite.gameObject.AddComponent<GradientColorComponent>();
            grad.playerId = __instance.PlayerId;
            grad.Initialize();
        }
        return false;
    }
}