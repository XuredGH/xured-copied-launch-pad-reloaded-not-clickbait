using HarmonyLib;
using LaunchpadReloaded.API.GameOptions;

namespace LaunchpadReloaded.Patches.Options;
[HarmonyPatch(typeof(AmongUsClient))]
public static class AmongUsClientPatch
{
    /// <summary>
    /// This patch is used for syncing game options when a player joins.
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("OnPlayerJoined")]
    public static void PlayerJoinedPatch()
    {
        if (!AmongUsClient.Instance.AmHost) return;
        CustomOptionsManager.SyncOptions();
    }
}