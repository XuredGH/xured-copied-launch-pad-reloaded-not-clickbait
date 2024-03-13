using System.Linq;
using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
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

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.Die))]
    public static void OnPlayerDie(PlayerControl __instance)
    {
        CustomGamemodeManager.ActiveMode.OnDeath(__instance);
    }
    
    [HarmonyPrefix]
    [HarmonyPatch("Start")]
    public static void StartPrefix(PlayerControl __instance)
    {
        var gradColorComponent = __instance.gameObject.AddComponent<PlayerGradientData>();
        if (__instance.AmOwner)
        {
            gradColorComponent.gradientColor = GradientManager.LocalGradientId;
            GradientManager.RpcSetGradient(__instance,GradientManager.LocalGradientId);
            Debug.LogError("Sent gradient");
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.SetPlayerMaterialColors))]
    public static void SetPlayerMaterialColorsPrefix(PlayerControl __instance, [HarmonyArgument(0)] Renderer renderer)
    {
        var playerGradient = __instance.GetComponent<PlayerGradientData>();
        if (playerGradient)
        {
            renderer.GetComponent<GradientColorComponent>().SetColor(__instance.Data.DefaultOutfit.ColorId, playerGradient.gradientColor);
        }
    }
}