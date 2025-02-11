using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Options;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using MiraAPI.GameOptions;
using MiraAPI.Roles;

namespace LaunchpadReloaded.Patches.Roles;
[HarmonyPatch]
public static class RoleManagerPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SetRole))]
    public static void SetRolePostfix(PlayerControl targetPlayer)
    {
        var tagManager = targetPlayer.GetTagManager();

        if (tagManager == null)
        {
            return;
        }

        var existingRoleTag = tagManager.GetTagByName("Role");
        if (existingRoleTag.HasValue)
        {
            tagManager.RemoveTag(existingRoleTag.Value);
        }

        var role = targetPlayer.Data.Role;
        var color = role is ICustomRole custom ? custom.RoleColor : role.TeamColor;
        var name = role.NiceName;

        if (role.IsDead && name == "STRMISS")
        {
            name = "Ghost";
        }

        var roleTag = new PlayerTag()
        {
            Name = "Role",
            Text = name,
            Color = color,
            IsLocallyVisible = (player) =>
            {
                var deadFlag = OptionGroupSingleton<GeneralOptions>.Instance.GhostsSeeRoles && PlayerControl.LocalPlayer.Data.IsDead;
                var plrRole = player.Data.Role;

                if (plrRole is ILaunchpadRole launchpadRole && (player.AmOwner || launchpadRole.CanSeeRoleTag()))
                {
                    return true;
                }

                if (player.AmOwner || deadFlag || PlayerControl.LocalPlayer.Data.Role.IsImpostor && plrRole.IsImpostor)
                {
                    return true;
                }

                return false;
            },
        };

        tagManager.AddTag(roleTag);
    }

}
