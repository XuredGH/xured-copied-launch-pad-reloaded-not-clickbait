using System.Collections.Generic;
using HarmonyLib;
using InnerNet;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(AmongUsClient))]
public static class AmongUsClientPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(AmongUsClient.OnPlayerJoined))]
    public static void PlayerJoinedPostfix(ClientData data)
    {
        if (!AmongUsClient.Instance.AmHost)
        {
            return;
        }

        CustomOptionsManager.SyncOptions(data.Id);
        CustomRoleManager.SyncRoleSettings(data.Id);

        var colorData = new Dictionary<byte, CustomColorData>();
        
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (GradientManager.TryGetColor(player.PlayerId, out var color))
            {
                colorData.Add(player.PlayerId, new CustomColorData((byte)player.Data.DefaultOutfit.ColorId, color));
            }
        }
        
        Rpc<RpcSyncAllColors>.Instance.SendTo(data.Id, colorData);
    }

    /// <summary>
    /// So the host uses HIS options and not others, also calls the valuechanged event for gamemodes/voting types when the game is made
    /// </summary>  
    [HarmonyPostfix]
    [HarmonyPatch(nameof(AmongUsClient.CreatePlayer))]
    public static void CreatePlayerPatch(AmongUsClient __instance, ClientData clientData)
    {
        if (__instance.AmHost && clientData.Character.AmOwner)
        {
            CustomOptionsManager.UpdateToConfig();
        }
    }
}