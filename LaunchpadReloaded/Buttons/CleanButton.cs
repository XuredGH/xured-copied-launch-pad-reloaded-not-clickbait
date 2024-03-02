﻿using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Roles;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class CleanButton : CustomActionButton
{
    public override string Name => "CLEAN";
    public override float Cooldown => 5;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override Sprite Sprite => LaunchpadReloadedPlugin.Bundle.LoadAsset<Sprite>("Clean.png");

    private DeadBody _target;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is JanitorRole;
    }

    public override void Update(PlayerControl playerControl)
    {
        base.Update(playerControl);
        playerControl.UpdateBodies(Color.yellow, ref _target);
    }

    protected override bool CanUse()
    {
        var playerCounts = GameManager.Instance.LogicFlow.GetPlayerCounts();
        return base.CanUse() && _target is not null && (playerCounts.Item3 == 1 || playerCounts.Item2 > 1);
    }

    protected override void OnClick()
    {
        _target.Reported = true;
        _target.enabled = false;
        foreach (var bodyRenderer in _target.bodyRenderers)
        {
            bodyRenderer.enabled = false;
        }
    }
}