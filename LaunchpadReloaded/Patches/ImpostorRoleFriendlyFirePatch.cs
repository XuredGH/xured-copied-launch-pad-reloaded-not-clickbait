﻿using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(ImpostorRole), nameof(ImpostorRole.IsValidTarget))]
public static class ImpostorRoleFriendlyFirePatch
{
    public static bool Prefix(ImpostorRole __instance, [HarmonyArgument(0)] GameData.PlayerInfo target, ref bool __result)
    {
        CustomGameModeManager.ActiveMode.CanKill(out var runOriginal, out var result, target.Object);
        if (LaunchpadGameOptions.Instance.FriendlyFire.Value || (!runOriginal && result))
        {
            // cant be arsed to use a reverse patch, so this is copied from mono VVV
            __result = target is { Disconnected: false, IsDead: false } &&
                       target.PlayerId != __instance.Player.PlayerId && !(target.Role == null) &&
                       !(target.Object == null) && !target.Object.inVent && !target.Object.inMovingPlat;
            return false;
        }

        return true;
    }
}