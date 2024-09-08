using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class InstinctButton : CustomActionButton
{
    public override string Name => "INSTINCT";
    public override float Cooldown =>  OptionGroupSingleton<DetectiveOptions>.Instance.InstinctCooldown;
    public override float EffectDuration => OptionGroupSingleton<DetectiveOptions>.Instance.InstinctDuration;
    public override int MaxUses => (int)OptionGroupSingleton<DetectiveOptions>.Instance.InstinctUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.InstinctButton;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is DetectiveRole;
    }

    public override void OnEffectEnd()
    {
        LaunchpadPlayer.LocalPlayer.showFootsteps = false;
    }
    protected override void OnClick()
    {
        LaunchpadPlayer.LocalPlayer.showFootsteps = true;
    }
}