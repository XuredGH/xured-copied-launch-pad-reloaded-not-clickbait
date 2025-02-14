using LaunchpadReloaded.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using System;

namespace LaunchpadReloaded.Options.Roles.Neutral;

public class JesterOptions : AbstractOptionGroup
{
    public override string GroupName => "Jester";

    public override Type AdvancedRole => typeof(JesterRole);

    [ModdedToggleOption("Can Use Vents")]
    public bool CanUseVents { get; set; } = true;
}