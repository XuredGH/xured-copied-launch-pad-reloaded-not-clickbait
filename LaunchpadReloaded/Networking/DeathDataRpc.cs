using System;
using System.Collections.Generic;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Networking;

public static class DeathDataRpc
{
    public static void RpcDeathData(this PlayerControl playerControl, PlayerControl killer, byte[] suspectIds)
    {
        var suspects = new List<PlayerControl>();
        foreach (var suspectId in suspectIds)
        {
            var suspect = GameData.Instance.GetPlayerById(suspectId);
            if (suspect != null)
            {
                suspects.Add(suspect.Object);
            }
        }

        var deathData = new DeathData(DateTime.UtcNow, killer, suspects);
        playerControl.GetModifierComponent()!.AddModifier(deathData);
    }
}