﻿using HarmonyLib;
using LaunchpadReloaded.Gamemodes;
using LaunchpadReloaded.Options;
using MiraAPI.GameModes;
using MiraAPI.GameOptions;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Roles;

/// <summary>
/// Set nametag color depending on option
/// </summary>
[HarmonyPatch(typeof(PlayerNameColor))]
public static class NameTagPatch
{
    [HarmonyPrefix, HarmonyPatch(nameof(PlayerNameColor.Get), typeof(RoleBehaviour))]
    public static bool GetPatch([HarmonyArgument(0)] RoleBehaviour otherPlayerRole, ref Color __result)
    {
        if (PlayerControl.LocalPlayer.Data.IsDead && ModdedGroupSingleton<GeneralOptions>.Instance.GhostsSeeRoles)
        {
            __result = otherPlayerRole.NameColor;
            return false;
        }
        
        if (CustomGameModeManager.ActiveMode is BattleRoyale)
        {
            return true;
        }

        __result = PlayerControl.LocalPlayer.Data?.Role == otherPlayerRole ? otherPlayerRole.NameColor : Color.white;

        return false;
    }
}