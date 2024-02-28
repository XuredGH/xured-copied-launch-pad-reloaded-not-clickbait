using HarmonyLib;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.API.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(RoleOptionSetting))]
public static class RoleOptionSettingsPatch
{
    private static readonly Sprite Empty = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded");
    
    [HarmonyPrefix]
    [HarmonyPatch("ShowRoleDetails")]
    public static bool ShowRoleDetailsPrefix(RoleOptionSetting __instance)
    {
        if (__instance.Role is not ICustomRole) return true;
        GameSettingMenu.Instance.RoleName.text = __instance.Role.NiceName;
        GameSettingMenu.Instance.RoleBlurb.text = __instance.Role.BlurbLong;
        GameSettingMenu.Instance.RoleIcon.sprite = Empty;
        return false;
    }
}