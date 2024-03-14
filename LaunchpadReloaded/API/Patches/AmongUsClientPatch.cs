using HarmonyLib;
using LaunchpadReloaded.API.GameOptions;

namespace LaunchpadReloaded.API.Patches;
[HarmonyPatch(typeof(AmongUsClient))]
public static class AmongUsClientPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(AmongUsClient.OnPlayerJoined))]
    public static void RpcSyncSettingsPostfix()
    {
        if (!AmongUsClient.Instance.AmHost) return;
        CustomOptionsManager.SyncOptions();
    }
}