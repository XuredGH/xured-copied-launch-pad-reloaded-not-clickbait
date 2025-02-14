using MiraAPI.Modifiers.Types;
using MiraAPI.Networking;
using System;

namespace LaunchpadReloaded.Modifiers;

public class PoisonModifier : TimedModifier
{
    public override string ModifierName => "Poisoned";
    public override bool HideOnUi => true;
    public override bool AutoStart => true;

    public override float Duration { get; }
    public DateTime DeathTime { get; init; }
    public PlayerControl Killer { get; init; }

    public PoisonModifier(PlayerControl killer, int time)
    {
        DeathTime = DateTime.UtcNow;
        Killer = killer;
        Duration = time;
    }

    public override void OnTimerComplete()
    {
        if (Player is null || Killer is null || Player.Data.IsDead || Killer.Data.IsDead || MeetingHud.Instance is not null)
        {
            return;
        }

        Killer.RpcCustomMurder(Player, resetKillTimer: false, createDeadBody: true, teleportMurderer: false, showKillAnim: false);
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }
}
