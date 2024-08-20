using System;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Roles.Options;

public class CaptainOptions : IModdedOptionGroup
{
    public string GroupName => "Captain";

    public Type AdvancedRole => typeof(CaptainRole);

    [ModdedNumberOption("Meeting Cooldown", 0, 120, 5, NumberSuffixes.Seconds)]
    public float CaptainMeetingCooldown { get; set; } = 45;

    [ModdedNumberOption("Meeting Uses", 1, 5)]
    public int CaptainMeetingCount { get; set; } = 3;

    [ModdedNumberOption("Zoom Cooldown", 5, 60, 2.5f, NumberSuffixes.Seconds)]
    public float ZoomCooldown { get; set; } = 30;

    [ModdedNumberOption("Zoom Duration", 5, 25, 1, NumberSuffixes.Seconds)]
    public float ZoomDuration { get; set; } = 10;

    [ModdedNumberOption("Zoom Distance", 4, 15)]
    public float ZoomDistance { get; set; } = 6;
}