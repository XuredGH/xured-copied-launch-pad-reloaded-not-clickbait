using MiraAPI.Modifiers;
using System;
using System.Collections.Generic;

namespace LaunchpadReloaded.Modifiers;

[RegisterModifier]
public class DeathData : BaseModifier
{
    public override string ModifierName => "DeathData";

    public override bool HideOnUi => false;
    public DateTime DeathTime { get; init; }
    public PlayerControl Killer { get; init; }
    public List<PlayerControl> Suspects { get; init; }

    public DeathData(DateTime deathTime, PlayerControl killer, List<PlayerControl> suspects)
    {
        DeathTime = deathTime;
        Killer = killer;
        Suspects = suspects;
    }
}