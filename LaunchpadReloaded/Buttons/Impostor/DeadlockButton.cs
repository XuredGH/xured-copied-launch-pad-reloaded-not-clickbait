using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles.Impostor;
using LaunchpadReloaded.Roles.Impostor;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons.Impostor;

public class DeadlockButton : BaseLaunchpadButton
{
    public override string Name => "Deadlock";
    public override float Cooldown => (int)OptionGroupSingleton<HitmanOptions>.Instance.DeadlockCooldown;
    public override float EffectDuration => OptionGroupSingleton<HitmanOptions>.Instance.DeadlockDuration;
    public override int MaxUses => (int)OptionGroupSingleton<HitmanOptions>.Instance.DeadlockUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.InjectButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => false;
    public override bool Enabled(RoleBehaviour? role) => role is HitmanRole;

    protected override void OnClick()
    {
        var hitmanRole = PlayerControl.LocalPlayer.Data.Role as HitmanRole;
        var manager = hitmanRole?.Manager;
        if (manager == null)
        {
            return;
        }

        if (manager.isDeadEyeActive)
        {
            hitmanRole?.Manager.StopDeadEye();
        }
        else
        {
            hitmanRole?.Manager.StartDeadEye();
        }
    }
}