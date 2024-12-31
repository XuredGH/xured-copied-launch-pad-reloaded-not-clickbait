using System;
using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using UnityEngine;

namespace LaunchpadReloaded.Roles;


[RegisterCustomRole]
public class OutcastGhostRole(IntPtr ptr) : CrewmateGhostRole(ptr), ICustomRole
{
    public string RoleName => "Outcast Ghost";
    public string RoleDescription => string.Empty;
    public string RoleLongDescription => string.Empty;
    public Color RoleColor => Color.gray;
    public ModdedRoleTeams Team => ModdedRoleTeams.Neutral;

    public CustomRoleConfiguration Configuration => new(this)
    {
        TasksCountForProgress = false,
        CanUseVent = false,
    };

    public override bool IsDead => true;
    public override bool IsAffectedByComms => false;

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        playerControl.ClearTasks();
        PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl).Text = $"{Color.gray.ToTextColor()}You are dead, you cannot do tasks.\nThere is no way to win. You have lost.";
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return new StringBuilder();
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return false;
    }
}