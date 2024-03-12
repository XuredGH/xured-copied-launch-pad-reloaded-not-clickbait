using Il2CppSystem.Data;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;
public class ScannerButton : CustomActionButton
{
    public override string Name => "Deploy Scanner";

    public override float Cooldown => (int)LaunchpadGameOptions.Instance.ScannerCooldown.Value;

    public override float EffectDuration => 0;

    public override int MaxUses => (int)LaunchpadGameOptions.Instance.MaxScanners.Value;

    public override Sprite Sprite => LaunchpadAssets.ScannerButton;

    public override bool Enabled(RoleBehaviour role) => role is TrackerRole;

    protected override void OnClick()
    {
        ScannerManager.RpcCreateScanner(PlayerControl.LocalPlayer, 
            PlayerControl.LocalPlayer.GetTruePosition().x, PlayerControl.LocalPlayer.GetTruePosition().y);
    }
}
