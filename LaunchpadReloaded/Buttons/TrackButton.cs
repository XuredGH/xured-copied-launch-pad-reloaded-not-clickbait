using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;
public class TrackButton : CustomActionButton
{
    public override string Name => "Place Tracker";
    public override float Cooldown => 0;
    public override float EffectDuration => 0;
    public override int MaxUses => 1;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.TrackButton;
    public PlayerControl CurrentTarget;

    public override bool Enabled(RoleBehaviour role) => role is TrackerRole;

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        base.FixedUpdate(playerControl);

        if (TrackingManager.Instance.TrackedPlayer && !TrackingManager.Instance.TrackerDisconnected && !PlayerControl.LocalPlayer.Data.IsHacked())
        {
            TrackingManager.Instance.TrackingUpdate();
            return;
        }

        if (UsesLeft > 0)
        {
            if (CurrentTarget)
            {
                CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetFloat(ShaderID.Outline, 0);
                CurrentTarget = null;
            }

            CurrentTarget = playerControl.GetClosestPlayer(true, 1.5f);

            if (CurrentTarget)
            {
                CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetFloat(ShaderID.Outline, 1);
                CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetColor(ShaderID.OutlineColor, Palette.CrewmateBlue);
            }
        }
    }

    public override bool CanUse() => CurrentTarget != null && !HackingManager.Instance.AnyPlayerHacked();

    protected override void OnClick()
    {
        TrackingManager.Instance.TrackedPlayer = CurrentTarget;
        CurrentTarget = null;
    }
}