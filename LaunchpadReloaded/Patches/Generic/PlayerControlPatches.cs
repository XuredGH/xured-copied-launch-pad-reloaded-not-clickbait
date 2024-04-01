using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using System.Linq;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(PlayerControl))]
public static class PlayerControlPatches
{
    /// <summary>
    /// Disable kill timer while janitor is dragging
    /// </summary>
    [HarmonyPostfix, HarmonyPatch(nameof(PlayerControl.IsKillTimerEnabled), MethodType.Getter)]
    public static void GetKillTimerEnabledPostfix(PlayerControl __instance, ref bool __result)
    {
        switch (__instance.Data.Role)
        {
            case JanitorRole:
                __result = __result && !DragManager.Instance.IsDragging(__instance.PlayerId);
                break;
        }
    }

    /// <summary>
    /// Unhack when players die, and trigger custom gamemode
    /// </summary>
    [HarmonyPostfix, HarmonyPatch(nameof(PlayerControl.Die))]
    public static void OnPlayerDie(PlayerControl __instance)
    {
        CustomGameModeManager.ActiveMode.OnDeath(__instance);
        __instance.GetLpPlayer().OnDeath();
    }

    /// <summary>
    /// Player control update
    /// </summary>
    [HarmonyPostfix, HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
    public static void UpdatePatch(PlayerControl __instance)
    {
        if (__instance.AmOwner)
        {
            foreach (var button in CustomButtonManager.CustomButtons)
            {
                if (!button.Enabled(__instance.Data.Role)) continue;
                button.UpdateHandler(__instance);
            }
        }

        if (__instance.Data is null || __instance.Data.Role is null) return;
        if (__instance.Data.Role is ICustomRole customRole) customRole.PlayerControlFixedUpdate(__instance);
    }

    /// <summary>
    /// Set gradient 
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("Start")]
    public static void StartPrefix(PlayerControl __instance)
    {
        var gradColorComponent = __instance.gameObject.AddComponent<PlayerGradientData>();
        gradColorComponent.playerId = __instance.PlayerId;
        if (__instance.AmOwner)
        {
            gradColorComponent.GradientColor = GradientManager.LocalGradientId;
            GradientManager.RpcSetGradient(__instance, GradientManager.LocalGradientId);
        }

        __instance.gameObject.AddComponent<LaunchpadPlayer>();
    }

    /// <summary>
    /// Patch to allow the same colors for players (if option enabled)
    /// </summary>
    [HarmonyPrefix, HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckColor))]
    public static bool CheckColorPatch(PlayerControl __instance, [HarmonyArgument(0)] byte colorId)
    {
        if (LaunchpadGameOptions.Instance.UniqueColors.Value) return true;
        __instance.RpcSetColor(colorId);
        return false;
    }

    /// <summary>
    /// Update gradient
    /// </summary>
    [HarmonyPostfix, HarmonyPatch(nameof(PlayerControl.SetPlayerMaterialColors))]
    public static void SetPlayerMaterialColorsPostfix(PlayerControl __instance, [HarmonyArgument(0)] Renderer renderer)
    {
        var playerGradient = __instance.GetComponent<PlayerGradientData>();
        if (playerGradient)
        {
            renderer.GetComponent<GradientColorComponent>().SetColor(__instance.Data.DefaultOutfit.ColorId, playerGradient.GradientColor);
        }
    }

    // TODO: Finish custom check color
    //[HarmonyPrefix]
    //[HarmonyPatch(nameof(PlayerControl.CheckColor))]
    public static bool CheckColorPrefix(PlayerControl __instance, [HarmonyArgument(0)] byte bodyColor)
    {
        var allPlayers = GameData.Instance.AllPlayers.ToArray();
        var num = 0;
        while (num++ < 100 &&
               (bodyColor >= Palette.PlayerColors.Length ||
                allPlayers.Any(p => !p.Disconnected &&
                                      p.PlayerId != __instance.PlayerId &&
                                      p.DefaultOutfit.ColorId == bodyColor)))
        {
            bodyColor = (byte)((bodyColor + 1) % Palette.PlayerColors.Length);
        }

        return false;
    }
}