using System.Collections.Generic;
using HarmonyLib;
using InnerNet;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking.Color;
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

        var colorData = new Dictionary<byte, CustomColorData>();
        
        foreach (var player in GameData.Instance.AllPlayers)
        {
            if (GradientManager.TryGetColor(player.PlayerId, out var color))
            {
                colorData.Add(player.PlayerId, new CustomColorData((byte)player.DefaultOutfit.ColorId, color));
            }
        }
        
        Rpc<RpcSyncAllColors>.Instance.SendTo(data.Id, colorData);
    }
}