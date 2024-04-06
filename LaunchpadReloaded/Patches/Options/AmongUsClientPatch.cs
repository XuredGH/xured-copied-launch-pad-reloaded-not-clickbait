using HarmonyLib;
using InnerNet;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;

namespace LaunchpadReloaded.Patches.Options;
[HarmonyPatch(typeof(AmongUsClient))]
public static class AmongUsClientPatch
{
    /// <summary>
    /// This patch is used for syncing game options when a player joins.
    /// </summary>
    [HarmonyPostfix, HarmonyPatch(nameof(AmongUsClient.OnPlayerJoined))]
    public static void PlayerJoinedPatch(ClientData data)
    {
        if (!AmongUsClient.Instance.AmHost || !data.InScene)
        {
            return;
        }

        CustomOptionsManager.SyncOptions(data.Id);
        CustomRoleManager.SyncRoleSettings(data.Id);
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