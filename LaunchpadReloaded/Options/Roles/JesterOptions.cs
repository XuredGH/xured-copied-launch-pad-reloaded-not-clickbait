using System;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Options.Roles;

public class JesterOptions : AbstractOptionGroup
{
    public override string GroupName => "Jester";
    
    public override Type AdvancedRole => typeof(JesterRole);
    
    [ModdedToggleOption("Can Use Vents")]
    public bool CanUseVents { get; set; } = true;
}