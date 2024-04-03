using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using System;
using System.Text;
using UnityEngine;

namespace LaunchpadReloaded.Roles;
[RegisterInIl2Cpp]
public class OutcastGhostRole(IntPtr ptr) : CrewmateGhostRole(ptr), ICustomRole
{
    public string RoleName => "Outcast Ghost";
    public ushort RoleId => (ushort)LaunchpadRoles.OutcastGhost;
    public string RoleDescription => string.Empty;
    public string RoleLongDescription => string.Empty;
    public Color RoleColor => Color.gray;
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public bool IsOutcast => true;
    public bool TasksCount => false;
    public bool CanUseVent => false;
    public bool IsGhostRole => true;
    public override bool IsDead => true;
    public override bool IsAffectedByComms => false;

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        playerControl.ClearTasks();
        PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl, 0).Text = $"{Color.gray.ToTextColor()}You are dead, you cannot do tasks.\nThere is no way to win. You have lost.";
    }
    public StringBuilder SetTabText() => null;
    public override bool DidWin(GameOverReason gameOverReason)
    {
        return false;
    }
}