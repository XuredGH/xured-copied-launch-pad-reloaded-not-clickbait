using MiraAPI.Modifiers;
using System;
using System.Collections.Generic;

namespace LaunchpadReloaded.Modifiers;

[RegisterModifier]
public class DeathData : BaseModifier
{
    public override string ModifierName => "DeathData";

    public override bool HideOnUi => true;
    public DateTime DeathTime { get; }
    public PlayerControl Killer { get; }
    public IEnumerable<PlayerControl> Suspects { get; }

    public DeathData(DateTime deathTime, PlayerControl killer, IEnumerable<PlayerControl> suspects)
    {
        DeathTime = deathTime;
        Killer = killer;
        Suspects = suspects;
    }
}