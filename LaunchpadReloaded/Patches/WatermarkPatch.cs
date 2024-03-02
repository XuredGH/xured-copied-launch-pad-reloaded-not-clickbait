using HarmonyLib;
using Reactor.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(PingTracker))]
public static class WatermarkPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PingTracker.Update))]
    public static bool Prefix(PingTracker __instance)
    {
        AspectPosition aspectPos = __instance.GetComponent<AspectPosition>();

        __instance.gameObject.SetActive(true);
        __instance.text.richText = true;
        __instance.text.text = "<color=#FF4050FF>All Of Us:</color> Launchpad \n<color=#7785CC>dsc.gg/allofus</color>";

        float x = HudManager.Instance.gameObject.GetComponentInChildren<FriendsListButton>() != null ? 4 : 2.3f;
        aspectPos.DistanceFromEdge = new Vector3(x, 0.1f, 0);

        return false;
    }
}