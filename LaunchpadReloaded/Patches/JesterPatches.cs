using HarmonyLib;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using System.Linq;

namespace LaunchpadReloaded.Patches;
[HarmonyPatch]
public static class JesterPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public static void SetUp(EndGameManager __instance)
    {
        bool didWin = TempData.winners.ToArray().Any((WinningPlayerData h) => h.IsYou);

        if (TempData.EndReason == (GameOverReason)CustomGameOverReason.JesterWins)
        {
            __instance.WinText.text += didWin ? "\n<size=30%>You Win.</size>" : "\n<size=30%>Jester Wins.</size>";
            __instance.BackgroundBar.material.SetColor("_Color", LaunchpadPalette.JesterColor);
            __instance.WinText.color = LaunchpadPalette.JesterColor;
            SoundManager.Instance.PlaySound(__instance.DisconnectStinger, false, 1f, null);
        };
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    public static void Begin(ExileController __instance)
    {
        if (__instance.exiled.Role is ICustomRole role)
        {
            if (role.GetCustomEjectionMessage(__instance.exiled) == null) return;
            __instance.completeString = role.GetCustomEjectionMessage(__instance.exiled);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static bool WrapUp(ExileController __instance)
    {
        if (__instance.exiled.Role is JesterRole)
        {
            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.JesterWins, false);
            return false;
        }
        return true;
    }
}
