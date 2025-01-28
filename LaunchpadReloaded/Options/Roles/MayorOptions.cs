using System;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Options.Roles;

public class MayorOptions : AbstractOptionGroup
{
    public override string GroupName => "Mayor";

    public override Type AdvancedRole => typeof(MayorRole);
    
    [ModdedNumberOption("Extra Votes", 1, 3)]
    public float ExtraVotes { get; set; } = 1;
}