using HarmonyLib;
using Il2CppSystem;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Networking.Color;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using MiraAPI.Utilities;
using Reactor.Networking.Rpc;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(PlayerControl))]
public static class PlayerControlPatches
{
    /// <summary>
    /// Disable kill timer while janitor is dragging
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.IsKillTimerEnabled), MethodType.Getter)]
    public static void GetKillTimerEnabledPostfix(PlayerControl __instance, ref bool __result)
    {
        switch (__instance.Data.Role)
        {
            case JanitorRole:
                __result = __result && !__instance.HasModifier<DragBodyModifier>();
                break;
        }
    }

    /// <summary>
    /// Use Custom check color RPC
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerControl.CmdCheckColor))]
    public static bool CheckColorPatch(PlayerControl __instance, [HarmonyArgument(0)] byte bodyColor)
    {
        if (AmongUsClient.Instance.AmHost)
        {
            Rpc<CustomCmdCheckColor>.Instance.Handle(__instance, new CustomColorData(bodyColor, (byte)GradientManager.LocalGradientId));
            return false;
        }
        
        Rpc<CustomCmdCheckColor>.Instance.SendTo(AmongUsClient.Instance.HostId, new CustomColorData(bodyColor, (byte)GradientManager.LocalGradientId));
        return false;
    }

    /// <summary>
    /// Player control update
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
    public static void UpdatePatch(PlayerControl __instance)
    {
        if (MeetingHud.Instance || __instance.Data is null)
        {
            return;
        }

        if (__instance.HasModifier<RevivedModifier>())
        {
            __instance.cosmetics.SetOutline(true, new Nullable<Color>(LaunchpadPalette.MedicColor));
        }
    }

    /// <summary>
    /// Set gradient 
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.Start))]
    public static void StartPrefix(PlayerControl __instance)
    {
        __instance.gameObject.AddComponent<PlayerGradientData>();
        __instance.GetModifierComponent().AddModifier<VoteData>();
    }

    /// <summary>
    /// Update gradient
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.SetPlayerMaterialColors))]
    public static void SetPlayerMaterialColorsPostfix(PlayerControl __instance, [HarmonyArgument(0)] Renderer renderer)
    {
        var playerGradient = __instance.GetComponent<PlayerGradientData>();
        if (playerGradient)
        {
            renderer.GetComponent<GradientColorComponent>().SetColor(__instance.Data.DefaultOutfit.ColorId, playerGradient.GradientColor);
        }
    }
}