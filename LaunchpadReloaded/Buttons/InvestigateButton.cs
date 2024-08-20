﻿using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class InvestigateButton : CustomActionButton
{
    public override string Name => "INVESTIGATE";
    public override float Cooldown => 1;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.InvestigateButton;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is DetectiveRole;
    }

    public override bool CanUse()
    {
        return base.CanUse() && LaunchpadPlayer.LocalPlayer.deadBodyTarget;
    }

    protected override void OnClick()
    {
        var gameObject = Object.Instantiate(LaunchpadAssets.DetectiveGame.LoadAsset(), HudManager.Instance.transform);
        var minigame = gameObject.GetComponent<JournalMinigame>();
        minigame.Open(LaunchpadPlayer.GetById(LaunchpadPlayer.LocalPlayer.deadBodyTarget.ParentId));
    }
}