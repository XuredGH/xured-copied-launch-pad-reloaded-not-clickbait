using HarmonyLib;
using LaunchpadReloaded.API.Roles;
using UnityEngine;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(RoleBehaviour))]
public static class RoleBehaviourPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(RoleBehaviour.TeamColor), MethodType.Getter)]
    public static bool PrefixTeamColorGetter(RoleBehaviour __instance, ref Color __result)
    {
        if (__instance is not ICustomRole behaviour) return true;
        __result = behaviour.RoleColor;
        return false;
    }
}