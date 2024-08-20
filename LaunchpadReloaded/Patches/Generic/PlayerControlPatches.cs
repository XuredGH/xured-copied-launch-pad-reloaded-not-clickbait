using HarmonyLib;
using Il2CppSystem;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking.Color;
using LaunchpadReloaded.Networking.Murder;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using MiraAPI.Hud;
using MiraAPI.Roles;
using Reactor.Networking.Rpc;
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
                __result = __result && !__instance.GetLpPlayer().Dragging;
                break;
        }
    }

    /// <summary>
    /// Use Custom murder RPC
    /// </summary>
    [HarmonyPrefix, HarmonyPatch(nameof(PlayerControl.CmdCheckMurder))]
    public static bool CheckMurderPrefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
    {
        __instance.isKilling = true;
        if (AmongUsClient.Instance.AmHost)
        {
            Rpc<CustomCmdCheckMurder>.Instance.Handle(__instance, target);
            return false;
        }
        
        Rpc<CustomCmdCheckMurder>.Instance.SendTo(AmongUsClient.Instance.HostId, target);
        return false;
    }


    /// <summary>
    /// Use Custom check color RPC
    /// </summary>
    [HarmonyPrefix, HarmonyPatch(nameof(PlayerControl.CmdCheckColor))]
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
    [HarmonyPostfix, HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
    public static void UpdatePatch(PlayerControl __instance)
    {
        if (MeetingHud.Instance || __instance.Data is null)
        {
            return;
        }

        if (__instance.GetLpPlayer().wasRevived)
        {
            __instance.cosmetics.SetOutline(true, new Nullable<Color>(LaunchpadPalette.MedicColor));
        }

        if (__instance.AmOwner)
        {
            foreach (var button in CustomButtonManager.CustomButtons)
            {
                if (!button.Enabled(__instance.Data.Role))
                {
                    continue;
                }

                button.FixedUpdateHandler(__instance);
            }
        }

        if (__instance.Data?.Role is ICustomRole customRole)
        {
            customRole.PlayerControlFixedUpdate(__instance);
        }
    }

    /// <summary>
    /// Set gradient 
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("Start")]
    public static void StartPrefix(PlayerControl __instance)
    {
        __instance.gameObject.AddComponent<PlayerGradientData>();
        __instance.gameObject.AddComponent<LaunchpadPlayer>();
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
}