using HarmonyLib;
using LaunchpadReloaded.API.GameModes;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
public static class WinningPatch
{
    public static void Prefix()
    {
        var gameMode = CustomGameModeManager.ActiveMode;
        if (gameMode.CalculateWinners() == null)
        {
            return;
        }

        TempData.winners.Clear();

        foreach (var winner in gameMode.CalculateWinners())
        {
            var data = new WinningPlayerData(winner);
            data.IsYou = winner.PlayerId == PlayerControl.LocalPlayer.PlayerId;
            TempData.winners.Add(data);
        }
    }
}