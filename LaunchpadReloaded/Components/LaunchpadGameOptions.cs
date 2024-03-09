using System;
using System.Linq;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.Roles;
using Reactor.Utilities.Attributes;
using UnityEngine;
using static Il2CppSystem.Linq.Expressions.Interpreter.CastInstruction.CastInstructionNoT;

namespace LaunchpadReloaded.Components;

public class LaunchpadGameOptions
{
    public static LaunchpadGameOptions Instance { get; private set; }
    
    public CustomToggleOption FriendlyFire { get; }
    public CustomNumberOption CaptainMeetingCooldown { get; }
    public CustomNumberOption CaptainMeetingCount { get; }
    public CustomStringOption Gamemodes { get; }

    public LaunchpadGameOptions()
    {
        if (Instance != null)
        {
            throw new Exception("Can't have more than one Launchpad Options");
        }

        FriendlyFire = new CustomToggleOption("Friendly Fire", false);

        CaptainMeetingCooldown = new CustomNumberOption("Captain Meeting Cooldown",
            45,
            0,
            120,
            5,
            NumberSuffixes.Seconds,
            role: typeof(CaptainRole));

        CaptainMeetingCount = new CustomNumberOption("Captain Meeting Uses",
            3,
            1,
            5,
            1,
            NumberSuffixes.None,
            role: typeof(CaptainRole));

        Action<int> changed = i =>
        {
            CustomGamemodeManager.SetGamemode(i);
        };

        Gamemodes = new CustomStringOption("Gamemode", ["Default", "Battle Royale"], changed);

        Instance = this;
    }
}