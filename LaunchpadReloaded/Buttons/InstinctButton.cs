using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Roles.Options;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class InstinctButton : CustomActionButton
{
    public override string Name => "INSTINCT";
    public override float Cooldown =>  ModdedGroupSingleton<DetectiveOptions>.Instance.InstinctCooldown;
    public override float EffectDuration => ModdedGroupSingleton<DetectiveOptions>.Instance.InstinctDuration;
    public override int MaxUses => (int)ModdedGroupSingleton<DetectiveOptions>.Instance.InstinctUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.InstinctButton;

    public override bool Enabled(RoleBehaviour role)
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