using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class InstinctButton : BaseLaunchpadButton
{
    public override string Name => "INSTINCT";
    public override float Cooldown =>  OptionGroupSingleton<DetectiveOptions>.Instance.InstinctCooldown;
    public override float EffectDuration => OptionGroupSingleton<DetectiveOptions>.Instance.InstinctDuration;
    public override int MaxUses => (int)OptionGroupSingleton<DetectiveOptions>.Instance.InstinctUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.InstinctButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => true;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is DetectiveRole;
    }

    public override void OnEffectEnd()
    {
        PlayerControl.LocalPlayer.GetModifierComponent()!.AddModifier<FootstepsModifier>();
    }

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.GetModifierComponent()!.RemoveModifier<FootstepsModifier>();
    }
}