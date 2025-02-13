using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class SwapButton : BaseLaunchpadButton
{
    public override string Name => "Swap";
    public override float Cooldown => OptionGroupSingleton<SwapshifterOptions>.Instance.SwapCooldown;
    public override float EffectDuration => OptionGroupSingleton<SwapshifterOptions>.Instance.SwapDuration;
    public override int MaxUses => (int)OptionGroupSingleton<SwapshifterOptions>.Instance.SwapUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.SwapButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => false;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is SwapshifterRole;
    }

    private PlayerControl? _currentTarget = null;

    public override void OnEffectEnd()
    {
        if (_currentTarget != null)
        {
            var currentPos2 = _currentTarget.GetTruePosition();
            _currentTarget.NetTransform.RpcSnapTo(PlayerControl.LocalPlayer.GetTruePosition());
            PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(currentPos2);

            PlayerControl.LocalPlayer.RpcShapeshift(PlayerControl.LocalPlayer, false);

            _currentTarget = null;
        }
    }

    protected override void OnClick()
    {
        CustomPlayerMenu playerMenu = CustomPlayerMenu.Create();
        playerMenu.Begin(plr => !plr.AmOwner && !plr.Data.IsDead && !plr.Data.Disconnected &&
        (!plr.Data.Role.IsImpostor || OptionGroupSingleton<SwapshifterOptions>.Instance.CanSwapImpostors), plr =>
        {
            _currentTarget = plr;

            var currentPos = _currentTarget.GetTruePosition();

            _currentTarget.NetTransform.RpcSnapTo(PlayerControl.LocalPlayer.GetTruePosition());
            PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(currentPos);

            PlayerControl.LocalPlayer.RpcShapeshift(_currentTarget, false);

            playerMenu.Close();
        });
    }
}