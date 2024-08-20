using System.Linq;
using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using MiraAPI.Roles;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Patches.Roles.Jester;
[HarmonyPatch]
public static class JesterPatches
{
    /// <summary>
    /// Custom end screen with jester color and text
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public static void SetUp(EndGameManager __instance)
    {
        var didWin = EndGameResult.CachedWinners.ToArray().Any(h => h.IsYou);

        if (EndGameResult.CachedGameOverReason != (GameOverReason)GameOverReasons.JesterWins)
        {
            return;
        }

        __instance.WinText.text += didWin ? "\n<size=30%>You Win.</size>" : "\n<size=30%>Jester Wins.</size>";
        __instance.BackgroundBar.material.SetColor(ShaderID.Color, LaunchpadPalette.JesterColor);
        __instance.WinText.color = LaunchpadPalette.JesterColor;
        SoundManager.Instance.PlaySound(__instance.DisconnectStinger, false);
    }
    /// <summary>
    /// Custom ejection text for roles
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    public static void Begin(ExileController __instance)
    {
        if (__instance.exiled && __instance.exiled.Role is not CrewmateRole or ImpostorRole or ICustomRole)
        {
            __instance.completeString = $"{__instance.exiled.PlayerName} was The {__instance.exiled.Role.NiceName}";
        }
    }

    /// <summary>
    /// If Jester gets voted out, end game
    /// </summary>
    [HarmonyPrefix, HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static bool WrapUp(ExileController __instance)
    {
        if (TutorialManager.InstanceExists)
        {
            return true;
        }

        if (__instance.exiled?.Role != null && __instance.exiled.Role is JesterRole)
        {
            GameManager.Instance.RpcEndGame((GameOverReason)GameOverReasons.JesterWins, false);
            return false;
        }
        return true;
    }
}
