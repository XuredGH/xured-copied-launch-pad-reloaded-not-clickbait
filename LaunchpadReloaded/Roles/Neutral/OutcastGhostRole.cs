﻿using MiraAPI.Roles;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Neutral;

public class OutcastGhostRole(IntPtr ptr) : RoleBehaviour(ptr), IOutcastRole
{
    public string RoleName => "Outcast Ghost";
    public string RoleDescription => string.Empty;
    public string RoleLongDescription => string.Empty;
    public Color RoleColor => Color.gray;

    public CustomRoleConfiguration Configuration => new(this)
    {
        HideSettings = true,
        RoleHintType = RoleHintType.TaskHint,
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

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return false;
    }

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        return false;
    }
}