﻿using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class CallButton : CustomActionButton
{
    public override string Name => "CALL";
    public override float Cooldown => CaptainRole.CaptainMeetingCooldown.Value;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)CaptainRole.CaptainMeetingCount.Value;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.CallButton;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is CaptainRole;
    }

    public override bool CanUse() => !ZoomButton.IsZoom;

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.CmdReportDeadBody(null);
    }
}