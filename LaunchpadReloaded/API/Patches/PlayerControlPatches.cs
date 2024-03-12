using HarmonyLib;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(PlayerControl))]
public static class PlayerControlPatches
{      
    [HarmonyPostfix]
    [HarmonyPatch("FixedUpdate")]
    public static void FixedUpdatePostfix(PlayerControl __instance)
    {
        if (MeetingHud.Instance) return;

        if (__instance.Data.IsHacked())
        {
            string randomString = Helpers.RandomString(Helpers.Random.Next(4, 6), "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#!?$(???#@)$@@@@0000");
            __instance.cosmetics.SetName(randomString);
            __instance.cosmetics.SetNameMask(true);
        }

        if (!__instance.AmOwner) return;

        foreach (var button in CustomButtonManager.CustomButtons)
        {
            if (button.Enabled(__instance.Data.Role))
            {
                button.UpdateHandler(__instance);
            }
        }
        
        if (__instance.Data.Role is ICustomRole customRole)
        {
            customRole.PlayerControlFixedUpdate(__instance);
        }
    }
}