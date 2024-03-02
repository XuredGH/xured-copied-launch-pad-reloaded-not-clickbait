﻿using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
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

    public override bool Enabled(RoleBehaviour role)
    {
        return role is JanitorRole;
    }
    
    public override bool CanUse()
    {
        var playerCounts = GameManager.Instance.LogicFlow.GetPlayerCounts();
        return DeadBodyTarget is not null && (playerCounts.Item3 == 1 || playerCounts.Item2 > 1);
    }

    protected override void OnClick()
    {
        DeadBodyManager.RpcHideBody(ShipStatus.Instance, DeadBodyTarget.ParentId);
    }
}