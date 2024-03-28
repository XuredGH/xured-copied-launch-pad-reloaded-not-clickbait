using HarmonyLib;
using InnerNet;
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

    /// <summary>
    /// So the host uses HIS options and not others, also calls the valuechanged event for gamemodes/voting types when the game is made
    /// </summary>  
    [HarmonyPostfix, HarmonyPatch("CreatePlayer")]
    public static void CreatePlayerPatch([HarmonyArgument(0)] ClientData data)
    {
        if (AmongUsClient.Instance.AmHost && data.Character.AmOwner)
        {
            CustomOptionsManager.UpdateToConfig();
        }
    }
}