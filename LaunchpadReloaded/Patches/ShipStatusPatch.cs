using HarmonyLib;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.API.Patches;
[HarmonyPatch(typeof(ShipStatus))]
public static class ShipStatusPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ShipStatus.Begin))]
    public static void MapLoadingPatch(ShipStatus __instance)
    {
        HackingManager.RpcCreateNodes(__instance);
    }
}
