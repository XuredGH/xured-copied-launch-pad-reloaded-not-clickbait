using System;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Options.Roles;

public class MayorOptions : IModdedOptionGroup
{
    public string GroupName => "Mayor";

    public Type AdvancedRole => typeof(MayorRole);
    
    [ModdedNumberOption("Extra Votes", 1, 3)]
    public float ExtraVotes { get; set; } = 1;
}