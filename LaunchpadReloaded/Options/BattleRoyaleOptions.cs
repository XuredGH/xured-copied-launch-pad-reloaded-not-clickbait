using System;
using LaunchpadReloaded.Gamemodes;
using MiraAPI.GameModes;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;

namespace LaunchpadReloaded.Options;

public class BattleRoyaleOptions : IModdedOptionGroup
{
    public string GroupName => "Battle Royale Options";

    public Func<bool> GroupVisible => () => CustomGameModeManager.ActiveMode?.GetType() == typeof(BattleRoyale);

    [ModdedToggleOption("Use Seeker Character")] public bool SeekerCharacter { get; set; } = true;
    
    public ModdedToggleOption ShowKnife { get; } = new("Show Knife", true)
    {
        Visible = () => ModdedGroupSingleton<BattleRoyaleOptions>.Instance.SeekerCharacter
    };
}