using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Roles.Options;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class ShootButton : CustomActionButton
{
    public override string Name => "Shoot";
    public override float Cooldown => ModdedGroupSingleton<SheriffOptions>.Instance.ShotCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)ModdedGroupSingleton<SheriffOptions>.Instance.ShotsPerGame;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.ShootButton;
    
    private PlayerControl _currentTarget;

    public override bool Enabled(RoleBehaviour role) => role is SheriffRole;

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        base.FixedUpdate(playerControl);

        if (_currentTarget)
        {
            _currentTarget.cosmetics.currentBodySprite.BodySprite.material.SetColor(ShaderID.OutlineColor, Color.clear);
            _currentTarget.cosmetics.currentBodySprite.BodySprite.material.SetFloat(ShaderID.Outline, 0);
            _currentTarget = null;
        }

        if (UsesLeft <= 0)
        {
            return;
        }
        
        _currentTarget = Utilities.Extensions.GetClosestPlayer(playerControl, true, GameManager.Instance.LogicOptions.GetKillDistance());

        if (!_currentTarget)
        {
            return;
        }
        
        _currentTarget.cosmetics.currentBodySprite.BodySprite.material.SetFloat(ShaderID.Outline, 1);
        _currentTarget.cosmetics.currentBodySprite.BodySprite.material.SetColor(ShaderID.OutlineColor, LaunchpadPalette.SheriffColor);
    }

    public override bool CanUse() => base.CanUse() && _currentTarget;

    protected override void OnClick()
    {
        if (_currentTarget.Data.Role.TeamType == RoleTeamTypes.Impostor || (ModdedGroupSingleton<SheriffOptions>.Instance.ShouldCrewmateDie && _currentTarget.Data.Role.TeamType == RoleTeamTypes.Crewmate))
        {
            PlayerControl.LocalPlayer.CmdCheckMurder(_currentTarget);
        }

        if (_currentTarget.Data.Role.TeamType == RoleTeamTypes.Crewmate && !TutorialManager.InstanceExists)
        {
            PlayerControl.LocalPlayer.CmdCheckMurder(PlayerControl.LocalPlayer);
        }

        _currentTarget = null;
    }
}