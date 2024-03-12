using System;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.Roles;
using Reactor.Utilities.Attributes;

namespace LaunchpadReloaded.Components;

public class LaunchpadGameOptions
{
    public static LaunchpadGameOptions Instance { get; private set; }

    public CustomToggleOption FriendlyFire;
    public CustomNumberOption CaptainMeetingCooldown;
    public CustomNumberOption CaptainMeetingCount;
    public CustomStringOption Gamemodes;;
    public CustomOptionGroup GeneralGroup;
    public CustomOptionGroup CaptainGroup;
    public CustomOptionGroup BattleRoyaleGroup;
    public CustomToggleOption SeekerCharacter;
    public CustomToggleOption ShowKnife;

    public LaunchpadGameOptions()
    {
        if (Instance != null)
        {
            throw new Exception("Can't have more than one Launchpad Options");
        }

        Gamemodes = new CustomStringOption("Gamemodes", ["Default", "Battle Royale"]);
        FriendlyFire = new CustomToggleOption("Friendly Fire", false);

        GeneralGroup = new CustomOptionGroup("General Options",
            toggleOpt: [FriendlyFire, new CustomToggleOption("Test", false)],
            stringOpt: [Gamemodes,],
            numberOpt: []);

        SeekerCharacter = new CustomToggleOption("Use Seeker Character", true);
        ShowKnife = new CustomToggleOption("Show Knife", true);

        BattleRoyaleGroup = new CustomOptionGroup("Battle Royale Options",
            toggleOpt: [SeekerCharacter, ShowKnife],
            stringOpt: [],
            numberOpt: []);
        BattleRoyaleGroup.Hidden = () => Gamemodes.Value != "Battle Royale";

        Instance = this;
    }

    public void CreateRoleOptions()
    {
        // Captain
        CaptainMeetingCooldown = new CustomNumberOption("Captain Meeting Cooldown",
            defaultValue: 45,
            range: new FloatRange(0, 120),
            increment: 5,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(CaptainRole));

        CaptainMeetingCount = new CustomNumberOption("Captain Meeting Uses",
            defaultValue: 3,
            range: new FloatRange(1, 5),
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(CaptainRole));

        CaptainGroup = new CustomOptionGroup($"{Palette.CrewmateBlue.ToTextColor()}Captain</color>",
            numberOpt: [CaptainMeetingCooldown, CaptainMeetingCount],
            stringOpt: [],
            toggleOpt: [], role: typeof(CaptainRole));
    }
}