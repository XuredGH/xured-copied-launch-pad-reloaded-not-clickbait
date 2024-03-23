using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches.Gamemodes;

/// <summary>
/// Allow Impostors to kill each other if can kill is enabled in gamemode or friendly fire is toggled on
/// </summary>
[HarmonyPatch(typeof(ImpostorRole), "IsValidTarget")]
public static class ImpostorValidTargetPatch
{
    public static bool Prefix(ImpostorRole __instance, [HarmonyArgument(0)] GameData.PlayerInfo target, ref bool __result)
    {
        CustomGameModeManager.ActiveMode.CanKill(out var runOriginal, out var result, target.Object);
        if (LaunchpadGameOptions.Instance.FriendlyFire.Value || !runOriginal && result)
        {
            __result = target is { Disconnected: false, IsDead: false } &&
                       target.PlayerId != __instance.Player.PlayerId && !(target.Role == null) &&
                       !(target.Object == null) && !target.Object.inVent && !target.Object.inMovingPlat;
            return false;
        }

        return true;
    }
}