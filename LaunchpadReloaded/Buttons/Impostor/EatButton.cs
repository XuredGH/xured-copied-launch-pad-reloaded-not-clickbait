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

public class EatButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Devour";
    public override float Cooldown => OptionGroupSingleton<DevourerOptions>.Instance.EatCooldown;
    public override float EffectDuration => 0f;// OptionGroupSingleton<DevourerOptions>.Instance.DevouredTime;
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
        return target != null && !target.HasModifier<EatenModifier>();
    }

    public override void SetOutline(bool active)
    {
        Target?.cosmetics.SetOutline(active, new Nullable<Color>(LaunchpadPalette.DevourerColor));
    }

    public override bool CanUse()
    {
        return base.CanUse();
    }

    protected override void OnClick()
    {
        Target?.RpcAddModifier<EatenModifier>();
    }
}