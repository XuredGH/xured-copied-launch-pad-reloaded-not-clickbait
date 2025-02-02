using HarmonyLib;
using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using TMPro;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Roles;

[HarmonyPatch]
public static class ShowRoleNamePatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerNameColor), nameof(PlayerNameColor.Set), typeof(NetworkedPlayerInfo), typeof(TextMeshPro))]
    public static void MeetingHudChangePatch(NetworkedPlayerInfo playerInfo, TextMeshPro name)
    {
        if (playerInfo is null || playerInfo.IsDead || playerInfo.Disconnected)
        {
            return;
        }

        var role = playerInfo.Role;
        var deadFlag = OptionGroupSingleton<GeneralOptions>.Instance.GhostsSeeRoles && PlayerControl.LocalPlayer.Data.IsDead;
        var color = role is ICustomRole custom ? custom.RoleColor : role.TeamColor;

        if (playerInfo.Object.AmOwner || deadFlag || PlayerControl.LocalPlayer.Data.Role.IsImpostor && role.IsImpostor)
        {
            name.text += $"\n<size=85%>{color.ToTextColor()}{role.NiceName}</size></color>";
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static void PlayerUpdatePatch(PlayerControl __instance)
    {
        if (!ShipStatus.Instance || __instance.Data is null || __instance.Data.IsDead || __instance.Data.Disconnected)
        {
            if (__instance.cosmetics.nameText)
            {
                __instance.cosmetics.nameText.transform.localPosition = new Vector3(0, 0, 0);
                __instance.cosmetics.nameText.text = __instance.Data!.PlayerName;
            }
            return;
        }

        var role = __instance.Data.Role;
        var deadFlag = OptionGroupSingleton<GeneralOptions>.Instance.GhostsSeeRoles && PlayerControl.LocalPlayer.Data.IsDead;
        var color = role is ICustomRole custom ? custom.RoleColor : role.TeamColor;

        if (__instance.AmOwner || deadFlag || PlayerControl.LocalPlayer.Data.Role.IsImpostor && role.IsImpostor)
        {
            __instance.cosmetics.nameText.text = __instance.Data.PlayerName + $"\n<size=85%>{color.ToTextColor()}{role.NiceName}</size></color>";
            __instance.cosmetics.nameText.transform.localPosition = __instance.cosmetics.colorBlindText.gameObject.active ? new Vector3(0, 0.2f, 0) : new Vector3(0, 0, 0);
        }
    }
}