using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class CallButton : CustomActionButton
{
    public override string Name => "CALL";
    public override float Cooldown => 5;
    public override float EffectDuration => 0;
    public override int MaxUses => 3;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.CallButton;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is CaptainRole;
    }

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.CmdReportDeadBody(null);
    }
}