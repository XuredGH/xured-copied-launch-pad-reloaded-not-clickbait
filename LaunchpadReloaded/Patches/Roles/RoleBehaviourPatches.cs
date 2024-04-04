﻿using HarmonyLib;
using LaunchpadReloaded.API.Roles;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Roles;

[HarmonyPatch(typeof(RoleBehaviour))]
public static class RoleBehaviourPatches
{
    /// <summary>
    /// Update TeamColor text for Launchpad roles
    /// </summary>
    [HarmonyPrefix, HarmonyPatch("TeamColor", MethodType.Getter)]
    public static bool PrefixTeamColorGetter(RoleBehaviour __instance, ref Color __result)
    {
        if (__instance is not ICustomRole behaviour)
        {
            return true;
        }

        __result = behaviour.RoleColor;
        return false;
    }
}