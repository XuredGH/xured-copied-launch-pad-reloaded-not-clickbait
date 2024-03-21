using HarmonyLib;
using LaunchpadReloaded.API.GameModes;

namespace LaunchpadReloaded.Patches.Gamemodes;

[HarmonyPatch(typeof(EndGameManager), "SetEverythingUp")]
public static class WinningPatch
{
    /// <summary>
    /// Update the winners of a game, used for custom game options (ex in Battle Royale, last person standing wins)
    /// </summary>
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