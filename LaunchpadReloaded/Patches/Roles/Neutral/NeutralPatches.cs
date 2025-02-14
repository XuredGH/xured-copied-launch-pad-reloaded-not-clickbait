using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles.Neutral;
using MiraAPI.Utilities;
using System.Linq;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Roles.Neutral;

[HarmonyPatch]
public static class NeutralPatches
{
    /// <summary>
    /// Custom end screen with neutral colors and text depending on role
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public static void SetUp(EndGameManager __instance)
    {
        var didWin = EndGameResult.CachedWinners.ToArray().Any(h => h.IsYou);
        var customWin = true;
        var winText = string.Empty;
        var color = Color.clear;

        switch (EndGameResult.CachedGameOverReason)
        {
            case (GameOverReason)GameOverReasons.JesterWins:
                winText += didWin ? "\n<size=30%>You Win.</size>" : "\n<size=30%>Jester Wins.</size>";
                color = LaunchpadPalette.JesterColor;
                break;
            case (GameOverReason)GameOverReasons.ReaperWins:
                winText += didWin ? "\n<size=30%>You Win.</size>" : "\n<size=30%>Reaper Wins.</size>";
                color = LaunchpadPalette.ReaperColor;
                break;
            default:
                customWin = false;
                break;
        }

        if (customWin)
        {
            __instance.WinText.text += winText;
            __instance.BackgroundBar.material.SetColor(ShaderID.Color, color);
            __instance.WinText.color = color;
            SoundManager.Instance.PlaySound(__instance.DisconnectStinger, false);
        }
    }

    /// <summary>
    /// Jester win condition
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static bool WrapUp(ExileController __instance)
    {
        if (NotepadHud.Instance != null)
        {
            NotepadHud.Instance.UpdateAspectPos();
        }

        if (TutorialManager.InstanceExists)
        {
            return true;
        }

        if (__instance.initData.networkedPlayer != null && __instance.initData.networkedPlayer.Role != null
            && __instance.initData.networkedPlayer.Role is JesterRole)
        {
            GameManager.Instance.RpcEndGame((GameOverReason)GameOverReasons.JesterWins, false);
            return false;
        }

        return true;
    }
}
