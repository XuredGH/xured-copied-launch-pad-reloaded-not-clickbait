using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TMPro;

namespace LaunchpadReloaded.Patches.Roles;

//[HarmonyPatch]
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
            name.text += $"\n<size=85%>{color.ToTextColor()}{role.NiceName}</color></size>";

            if (playerInfo.Object.HasModifier<RevivedModifier>())
            {
                name.text += $" <size=65%>{LaunchpadPalette.MedicColor.ToTextColor()}(Revived)</color></size>";
            }
            return;
        }

        if (playerInfo.Object.HasModifier<RevivedModifier>())
        {
            name.text += $"\n<size=65%>{LaunchpadPalette.MedicColor.ToTextColor()}(Revived)</color></size>";
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static void PlayerUpdatePatch(PlayerControl __instance)
    {
        if (!ShipStatus.Instance || __instance.Data is null || __instance.Data.IsDead || __instance.Data.Disconnected || PlayerControl.LocalPlayer is null || PlayerControl.LocalPlayer.Data is null)
        {
            if (__instance.cosmetics.nameText)
            {
                // __instance.cosmetics.nameText.transform.localPosition = new Vector3(0, 0, 0);
                __instance.cosmetics.nameText.text = __instance.Data!.PlayerName;
            }
            return;
        }

        var role = __instance.Data.Role;
        var deadFlag = OptionGroupSingleton<GeneralOptions>.Instance.GhostsSeeRoles && PlayerControl.LocalPlayer.Data.IsDead;
        var color = role is ICustomRole custom ? custom.RoleColor : role.TeamColor;

        var nameText = __instance.Data.PlayerName;

        if (__instance.AmOwner || deadFlag || PlayerControl.LocalPlayer.Data.Role.IsImpostor && role.IsImpostor)
        {
            nameText += $"\n<size=85%>{color.ToTextColor()}{role.NiceName}</size></color>";
            if (__instance.HasModifier<RevivedModifier>())
            {
                nameText += $" <size=65%>{LaunchpadPalette.MedicColor.ToTextColor()}(Revived)</color></size>";
            }

            __instance.cosmetics.nameText.text = nameText;
            // __instance.cosmetics.nameText.transform.localPosition = __instance.cosmetics.colorBlindText.gameObject.active ? new Vector3(0, 0.2f, 0) : new Vector3(0, 0, 0);
            return;
        }

        if (__instance.HasModifier<RevivedModifier>())
        {
            nameText += $"\n<size=65%>{LaunchpadPalette.MedicColor.ToTextColor()}(Revived)</color></size>";
        }

        __instance.cosmetics.nameText.text = nameText;
    }
}