using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using MiraAPI.Utilities;
using System.Linq;

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
    /// If Jester gets voted out, end game
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

        if (__instance.initData.networkedPlayer?.Role && __instance.initData.networkedPlayer.Role is JesterRole)
        {
            GameManager.Instance.RpcEndGame((GameOverReason)GameOverReasons.JesterWins, false);
            return false;
        }

        return true;
    }
}
