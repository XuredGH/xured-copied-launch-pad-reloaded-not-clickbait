using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using System;

namespace LaunchpadReloaded.Components;

public class LaunchpadGameOptions
{
    public static LaunchpadGameOptions Instance { get; private set; }

    // General Options
    public CustomToggleOption FriendlyFire;
    public CustomStringOption Gamemodes;
    public CustomOptionGroup GeneralGroup;

    // Battle Royale
    public CustomToggleOption SeekerCharacter;
    public CustomToggleOption ShowKnife;
    public CustomOptionGroup BattleRoyaleGroup;

    // Captain Options
    public CustomNumberOption CaptainMeetingCooldown;
    public CustomNumberOption CaptainMeetingCount;
    public CustomOptionGroup CaptainGroup;

    // Hacker Options
    public CustomNumberOption HackCooldown;
    public CustomNumberOption HackUses;
    public CustomNumberOption MapCooldown;
    public CustomNumberOption MapDuration;
    public CustomOptionGroup HackerGroup;

    // Janitor Options
    public CustomNumberOption HideCooldown;
    public CustomNumberOption HideUses;
    public CustomOptionGroup JanitorGroup;

    // Tracker Options
    public CustomNumberOption TrackerPingTimer;
    public CustomNumberOption ScannerCooldown;
    public CustomNumberOption MaxScanners;
    public CustomOptionGroup TrackerGroup;

    public LaunchpadGameOptions()
    {
        if (Instance != null)
        {
            throw new Exception("Can't have more than one Launchpad Options");
        }

        Gamemodes = new CustomStringOption("Gamemodes", 0, ["Default", "Battle Royale"]);
        Gamemodes.ChangedEvent = i =>
        {
            CustomGamemodeManager.RpcSetGamemode(PlayerControl.LocalPlayer, i);
        };

        FriendlyFire = new CustomToggleOption("Friendly Fire", false);

        GeneralGroup = new CustomOptionGroup("General Options",
            toggleOpt: [FriendlyFire],
            stringOpt: [Gamemodes,],
            numberOpt: []);

        SeekerCharacter = new CustomToggleOption("Use Seeker Character", true);
        ShowKnife = new CustomToggleOption("Show Knife", true);

        BattleRoyaleGroup = new CustomOptionGroup("Battle Royale Options",
            toggleOpt: [SeekerCharacter, ShowKnife],
            stringOpt: [],
            numberOpt: []);
        BattleRoyaleGroup.Hidden = () => Gamemodes.Value != "Battle Royale";

        CreateRoleOptions();

        Instance = this;
    }

    public void CreateRoleOptions()
    {
        // Captain
        CaptainMeetingCooldown = new CustomNumberOption("Meeting Cooldown",
            defaultValue: 45,
            range: new FloatRange(0, 120),
            increment: 5,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(CaptainRole));

        CaptainMeetingCount = new CustomNumberOption("Meeting Uses",
            defaultValue: 3,
            range: new FloatRange(1, 5),
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(CaptainRole));

        CaptainGroup = new CustomOptionGroup($"{LaunchpadPalette.CaptainColor.ToTextColor()}Captain</color>",
            numberOpt: [CaptainMeetingCooldown, CaptainMeetingCount],
            stringOpt: [],
            toggleOpt: [], role: typeof(CaptainRole));

        // Hacker
        HackCooldown = new CustomNumberOption("Hack Cooldown",
            defaultValue: 60,
            range: new FloatRange(10, 300),
            increment: 10,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(HackerRole));

        HackUses = new CustomNumberOption("Hacks Per Game",
            defaultValue: 2,
            range: new FloatRange(1, 8),
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(HackerRole));

        MapCooldown = new CustomNumberOption("Map Cooldown",
            defaultValue: 10,
            range: new FloatRange(0, 40),
            increment: 3,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(HackerRole));

        MapDuration = new CustomNumberOption("Map Duration",
            defaultValue: 3,
            range: new FloatRange(1, 30),
            increment: 3,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(HackerRole));

        HackerGroup = new CustomOptionGroup($"{LaunchpadPalette.HackerColor.ToTextColor()}Hacker</color>",
            numberOpt: [HackCooldown, HackUses, MapCooldown, MapDuration],
            stringOpt: [],
            toggleOpt: [], role: typeof(HackerRole));

        // Janitor
        HideCooldown = new CustomNumberOption("Hide Bodies Cooldown",
            defaultValue: 5,
            range: new FloatRange(0, 120),
            increment: 5,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(JanitorRole));

        HideUses = new CustomNumberOption("Hide Bodies Uses",
            defaultValue: 3,
            range: new FloatRange(1, 10),
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(JanitorRole));

        JanitorGroup = new CustomOptionGroup($"{LaunchpadPalette.JanitorColor.ToTextColor()}Janitor</color>",
            numberOpt: [HideCooldown, HideUses],
            stringOpt: [],
            toggleOpt: [], role: typeof(JanitorRole));

        // Tracker
        TrackerPingTimer = new CustomNumberOption("Tracker Ping Timer",
            defaultValue: 7,
            range: new FloatRange(3, 30),
            increment: 1,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(TrackerRole));

        MaxScanners = new CustomNumberOption("Max Scanners",
            defaultValue: 3,
            range: new FloatRange(1, 15),
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(TrackerRole));

        ScannerCooldown = new CustomNumberOption("Place Scanner Cooldown",
            defaultValue: 5,
            range: new FloatRange(1, 20),
            increment: 2,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(TrackerRole));

        TrackerGroup = new CustomOptionGroup($"{LaunchpadPalette.TrackerColor.ToTextColor()}Tracker</color>",
            numberOpt: [TrackerPingTimer, MaxScanners, ScannerCooldown],
            stringOpt: [],
            toggleOpt: [], role: typeof(TrackerRole));
    }
}