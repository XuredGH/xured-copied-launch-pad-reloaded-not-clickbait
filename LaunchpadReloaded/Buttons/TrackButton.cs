using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;
public class TrackButton : CustomActionButton
{
    public override string Name => "Place Tracker";
    public override float Cooldown => 0;
    public override float EffectDuration => 0;
    public override int MaxUses => 1;
    public override Sprite Sprite => LaunchpadAssets.TrackButton;
    public PlayerControl CurrentTarget = null;

    public override bool Enabled(RoleBehaviour role) => role is TrackerRole;

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        base.FixedUpdate(playerControl);

        if (TrackingManager.Instance.TrackedPlayer && !TrackingManager.Instance.TrackerDisconnected && !PlayerControl.LocalPlayer.Data.IsHacked())
        {
            TrackingManager.Instance.TrackingUpdate();
            return;
        }

        if(UsesLeft > 0)
        {
            if (CurrentTarget != null)
            {
                CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", (float)(0));
                CurrentTarget = null;
            }

            this.CurrentTarget = playerControl.GetClosestPlayer(true, 1.5f);

            if (CurrentTarget != null)
            {
                CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", (float)(1));
                CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", Palette.CrewmateBlue);
            }
        }
    }

    public override bool CanUse() => CurrentTarget != null;

    protected override void OnClick()
    {
        TrackingManager.Instance.TrackedPlayer = CurrentTarget; 
        CurrentTarget = null;
    }
}