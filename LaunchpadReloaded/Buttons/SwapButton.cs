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
    public override float Cooldown => OptionGroupSingleton<SwapperOptions>.Instance.SwapCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)OptionGroupSingleton<SwapperOptions>.Instance.SwapUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.SwapButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => false;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is SwapperRole;
    }

    protected override void OnClick()
    {
        CustomPlayerMenu playerMenu = CustomPlayerMenu.Create();
        playerMenu.Begin(plr => !plr.AmOwner && !plr.Data.IsDead && !plr.Data.Disconnected &&
        (!plr.Data.Role.IsImpostor || OptionGroupSingleton<SwapperOptions>.Instance.CanSwapImpostors), plr =>
        {
            var currentPos = PlayerControl.LocalPlayer.GetTruePosition();
            var targetPos = plr.GetTruePosition();

            plr.NetTransform.RpcSnapTo(currentPos);
            PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(targetPos);

            playerMenu.Close();
        });
    }
}