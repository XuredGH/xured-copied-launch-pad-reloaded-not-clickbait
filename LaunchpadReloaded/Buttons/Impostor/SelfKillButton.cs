using Il2CppSystem;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Options;
using LaunchpadReloaded.Options.Roles.Impostor;
using LaunchpadReloaded.Roles.Impostor;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons.Impostor;

public class SelfKillButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "'Kill'";
    public override float Cooldown => OptionGroupSingleton<SurgeonOptions>.Instance.InjectCooldown;
    public override float EffectDuration => OptionGroupSingleton<SurgeonOptions>.Instance.PoisonDelay;
    public override int MaxUses => (int)OptionGroupSingleton<SurgeonOptions>.Instance.InjectUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.InjectButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => false;

    public override bool Enabled(RoleBehaviour? role) => role is DevourerRole;

    public override PlayerControl? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestPlayer(OptionGroupSingleton<FunOptions>.Instance.FriendlyFire, 1.1f);
    }

    public override bool IsTargetValid(PlayerControl? target)
    {
        return target != null;
    }

    public override void SetOutline(bool active)
    {
        Target?.cosmetics.SetOutline(active, new Nullable<Color>(Color.red));
    }

    public override bool CanUse()
    {
        return base.CanUse();
    }

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.RpcCustomMurder(PlayerControl.LocalPlayer, createDeadBody: true, teleportMurderer: true, playKillSound: true, resetKillTimer: true, showKillAnim: true);
    }
}