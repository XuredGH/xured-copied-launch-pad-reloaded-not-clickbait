using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.GameModes;
using LaunchpadReloaded.Utilities;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Patches;
[HarmonyPatch(typeof(PlayerNameColor))]
public static class NameTagPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerNameColor.Get), new Type[] { typeof(RoleBehaviour) })]
    public static bool GetPatch([HarmonyArgument(0)] RoleBehaviour otherPlayerRole, ref Color __result)
    {
        if (LaunchpadGameOptions.Instance.OnlyShowRoleColor.Value || CustomGameModeManager.ActiveMode is BattleRoyale)
        {
            return true;
        }

        if (PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.Data == null || PlayerControl.LocalPlayer.Data.Role == null || otherPlayerRole == null)
        {
            __result = Color.white;
        }

        if (PlayerControl.LocalPlayer.Data.Role == otherPlayerRole)
        {
            __result = otherPlayerRole.NameColor;
        }
        else
        {
            __result = Color.white;
        }

        return false;
    }
}