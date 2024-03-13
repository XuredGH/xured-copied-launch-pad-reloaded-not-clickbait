using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;
public class ScannerButton : CustomActionButton
{
    public override string Name => "Deploy Scanner";

    public override float Cooldown => (int)LaunchpadGameOptions.Instance.ScannerCooldown.Value;

    public override float EffectDuration => 0;

    public override int MaxUses => (int)LaunchpadGameOptions.Instance.MaxScanners.Value;

    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.ScannerButton;

    public override bool Enabled(RoleBehaviour role) => role is TrackerRole;

    protected override void OnClick()
    {
        ScannerManager.RpcCreateScanner(PlayerControl.LocalPlayer,
            PlayerControl.LocalPlayer.GetTruePosition().x, PlayerControl.LocalPlayer.GetTruePosition().y);
    }
}
