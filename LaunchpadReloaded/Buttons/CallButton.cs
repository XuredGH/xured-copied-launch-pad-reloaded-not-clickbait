using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class CallButton : CustomActionButton
{
    public override string Name => "CALL";
    public override float Cooldown => LaunchpadGameOptions.Instance.CaptainMeetingCooldown.Value;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)LaunchpadGameOptions.Instance.CaptainMeetingCount.Value;
    public override Sprite Sprite => LaunchpadAssets.CallButton;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is CaptainRole;
    }

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.CmdReportDeadBody(null);
    }
}