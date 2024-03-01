﻿using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class HideButton : CustomActionButton
{
    public override string Name => "HIDE";
    public override float Cooldown => 5;
    public override float EffectDuration => 0;
    public override int MaxUses => 3;
    public override string SpritePath => "Clean.png";

    public override bool CanUse()
    {
        return DeadBodyTarget is not null && DragManager.DraggingPlayers.ContainsKey(PlayerControl.LocalPlayer);
    }

    protected override void OnClick()
    {
        
    }

    public override bool Enabled(RoleBehaviour role)
    {
        return role is HitmanRole;
    }
}