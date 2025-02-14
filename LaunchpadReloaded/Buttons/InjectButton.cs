﻿using Il2CppSystem;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Networking.Roles;
using LaunchpadReloaded.Options;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class InjectButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Inject";
    public override float Cooldown => OptionGroupSingleton<SurgeonOptions>.Instance.InjectCooldown;
    public override float EffectDuration => OptionGroupSingleton<SurgeonOptions>.Instance.PoisonDelay;
    public override int MaxUses => (int)OptionGroupSingleton<SurgeonOptions>.Instance.InjectUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.InjectButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => false;

    public override bool Enabled(RoleBehaviour? role) => role is SurgeonRole;

    public override PlayerControl? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestPlayer(OptionGroupSingleton<FunOptions>.Instance.FriendlyFire,
            GameOptionsManager.Instance.normalGameHostOptions.KillDistance);
    }

    public override bool IsTargetValid(PlayerControl? target)
    {
        return base.IsTargetValid(target) && !target.HasModifier<PoisonModifier>();
    }

    public override void SetOutline(bool active)
    {
        Target?.cosmetics.SetOutline(active, new Nullable<Color>(LaunchpadPalette.SurgeonColor));
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }

        SoundManager.Instance.PlaySound(LaunchpadAssets.InjectSound.LoadAsset(), false, volume: 3);
        PlayerControl.LocalPlayer.RpcPoison(Target, (int)OptionGroupSingleton<SurgeonOptions>.Instance.PoisonDelay);

        ResetTarget();
    }
}