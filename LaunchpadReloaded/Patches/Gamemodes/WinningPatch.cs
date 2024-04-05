using System.Linq;
using HarmonyLib;
using LaunchpadReloaded.API.GameModes;

namespace LaunchpadReloaded.Patches.Gamemodes;

[HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
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

        foreach (var data in gameMode.CalculateWinners().Select(winner => new WinningPlayerData(winner)))
        {
            TempData.winners.Add(data);
        }
    }
}