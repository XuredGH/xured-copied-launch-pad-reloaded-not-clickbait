using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(GameManager))]
public static class GameManagerPatch
{
    [HarmonyPostfix, HarmonyPatch(nameof(GameManager.OnPlayerDeath))]
    public static void OnDeathPostfix(PlayerControl player)
    {
        CustomGameModeManager.ActiveMode.OnDeath(player);
        player.GetLpPlayer().OnDeath();
    }
}