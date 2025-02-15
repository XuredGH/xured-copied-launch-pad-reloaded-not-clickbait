using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(FollowerCamera), nameof(FollowerCamera.SetTarget))]
public static class BloomPatch
{
    public static void Postfix()
    {
        if (Camera.main == null)
        {
            return;
        }

        var bloom = Camera.main.GetComponent<Bloom>();
        if (bloom == null)
        {
            bloom = Camera.main.gameObject.AddComponent<Bloom>();
        }

        bloom.enabled = LaunchpadSettings.Instance?.Bloom.Enabled ?? false;
        bloom.SetBloomByMap();
    }
}