﻿using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
public static class WinningPatch
{
    public static void Prefix()
    {
        CustomGamemode gamemode = CustomGamemodeManager.ActiveMode;
        if (gamemode.CalculateWinners() == null) return;

        TempData.winners.Clear();

        foreach (var winner in gamemode.CalculateWinners())
        {
            WinningPlayerData data = new WinningPlayerData(winner);
            data.IsYou = winner.PlayerId == PlayerControl.LocalPlayer.PlayerId;
            TempData.winners.Add(data);
        }
    }
}