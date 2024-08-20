using System;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Roles.Options;

public class JesterOptions : IModdedOptionGroup
{
    public string GroupName => "Jester";
    
    public Type AdvancedRole => typeof(JesterRole);
    
    [ModdedToggleOption("Can Use Vents")]
    public bool CanUseVents { get; set; } = true;
}