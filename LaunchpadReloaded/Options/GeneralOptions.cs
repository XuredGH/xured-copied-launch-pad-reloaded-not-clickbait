using System;
using MiraAPI.GameModes;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Options;

public class GeneralOptions : IModdedOptionGroup
{
    public string GroupName => "General";
    
    public Func<bool> GroupVisible => CustomGameModeManager.IsDefault;

    [ModdedToggleOption("Ban Cheaters")] public bool BanCheaters { get; set; } = true;
    
    [ModdedToggleOption("Disable Meeting Teleport")] public bool DisableMeetingTeleport { get; set; } = false;
    
    [ModdedToggleOption("Ghosts See Roles")] public bool GhostsSeeRoles { get; set; } = false;

}