using HarmonyLib;
using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Roles;

/// <summary>
/// Set nametag color depending on option
/// </summary>
[HarmonyPatch(typeof(PlayerNameColor), nameof(PlayerNameColor.Get), typeof(RoleBehaviour))]
public static class NameTagColorPatch
{
    public static bool Prefix([HarmonyArgument(0)] RoleBehaviour otherPlayerRole, ref Color __result)
    {
        if (PlayerControl.LocalPlayer.Data.IsDead && OptionGroupSingleton<GeneralOptions>.Instance.GhostsSeeRoles)
        {
            __result = otherPlayerRole.NameColor;
            return false;
        }

        /*if (CustomGameModeManager.ActiveMode is BattleRoyale)
        {
            return true;
        }*/

        __result = PlayerControl.LocalPlayer.Data?.Role == otherPlayerRole ? otherPlayerRole.NameColor : Color.white;

        return false;
    }
}