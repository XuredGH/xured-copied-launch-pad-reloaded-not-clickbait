using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;
public class ShootButton : CustomActionButton
{
    public override string Name => "Shoot";
    public override float Cooldown => SheriffRole.ShootCooldown.Value;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)SheriffRole.Shots.Value;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.ShootButton;
    public PlayerControl CurrentTarget;

    public override bool Enabled(RoleBehaviour role) => role is SheriffRole;

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        base.FixedUpdate(playerControl);

        if (UsesLeft > 0)
        {
            if (CurrentTarget)
            {
                CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetColor(ShaderID.OutlineColor, Color.clear);
                CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetFloat(ShaderID.Outline, 0);
                CurrentTarget = null;
            }

            CurrentTarget = playerControl.GetClosestPlayer(true, GameManager.Instance.LogicOptions.GetKillDistance());

            if (CurrentTarget)
            {
                CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetFloat(ShaderID.Outline, 1);
                CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetColor(ShaderID.OutlineColor, LaunchpadPalette.SheriffColor);
            }
        }
        else if (UsesLeft <= 0 && CurrentTarget is not null)
        {
            CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetColor(ShaderID.OutlineColor, Color.clear);
            CurrentTarget.cosmetics.currentBodySprite.BodySprite.material.SetFloat(ShaderID.Outline, 0);
            CurrentTarget = null;
        }
    }

    public override bool CanUse() => CurrentTarget != null;

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.RpcMurderPlayer(CurrentTarget, true);
        if (CurrentTarget.Data.Role.TeamType == RoleTeamTypes.Crewmate && !TutorialManager.InstanceExists
            ) PlayerControl.LocalPlayer.RpcMurderPlayer(PlayerControl.LocalPlayer, true); ;

        CurrentTarget = null;
    }
}