using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;
public class ScannerButton : CustomActionButton
{
    public override string Name => "Deploy Scanner";

    public override float Cooldown => (int)TrackerRole.ScannerCooldown.Value;

    public override float EffectDuration => 0;

    public override int MaxUses => (int)TrackerRole.MaxScanners.Value;

    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.ScannerButton;

    public override bool Enabled(RoleBehaviour role) => role is TrackerRole;
    public override bool CanUse() => !HackingManager.Instance.AnyPlayerHacked();

    protected override void OnClick()
    {
        ScannerManager.RpcCreateScanner(PlayerControl.LocalPlayer,
            PlayerControl.LocalPlayer.GetTruePosition().x, PlayerControl.LocalPlayer.GetTruePosition().y);
    }
}
