using HarmonyLib;
using LaunchpadReloaded.API.GameOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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