using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using System.Text;

namespace LaunchpadReloaded.Utilities;

public static class Helpers
{
    public static bool ShouldCancelClick()
    {
        return DragManager.IsDragging(PlayerControl.LocalPlayer.PlayerId);
    }

    public static StringBuilder CreateForRole(ICustomRole role)
    {
        StringBuilder taskStringBuilder = new StringBuilder();
        taskStringBuilder.AppendLine($"{role.RoleColor.ToTextColor()}You are a <b>{role.RoleName}.</b></color>");
        taskStringBuilder.Append("<size=70%>");
        taskStringBuilder.AppendLine($"{role.RoleLongDescription}");
        return taskStringBuilder;
    }
}