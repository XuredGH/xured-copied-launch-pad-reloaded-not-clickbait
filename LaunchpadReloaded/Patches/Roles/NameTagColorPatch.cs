using HarmonyLib;
using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Roles;

[HarmonyPatch(typeof(PlayerNameColor))]
public static class NameTagColorPatch
{
    [HarmonyPriority(Priority.Last)]
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerNameColor.Get), typeof(RoleBehaviour))]
    public static bool GetPatch([HarmonyArgument(0)] RoleBehaviour otherPlayerRole, ref Color __result)
    {
        if (PlayerControl.LocalPlayer.Data.IsDead && OptionGroupSingleton<GeneralOptions>.Instance.GhostsSeeRoles)
        {
            __result = otherPlayerRole.NameColor;
            return false;
        }

        if (PlayerControl.LocalPlayer.Data!.Role.IsImpostor && otherPlayerRole.IsImpostor)
        {
            __result = otherPlayerRole.NameColor;
        }

        return false;
    }
}