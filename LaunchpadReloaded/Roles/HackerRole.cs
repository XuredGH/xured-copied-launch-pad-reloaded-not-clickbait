using System;
using System.Text;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class HackerRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Hacker";
    public ushort RoleId => (ushort)LaunchpadRoles.Hacker;
    public string RoleDescription => "Hack meetings and sabotage the crewmates";
    public string RoleLongDescription => "Hack crewmates and make them unable to do tasks\nAnd view the admin map from anywhere!";
    public Color RoleColor => LaunchpadPalette.HackerColor;
    public RoleTeamTypes Team => RoleTeamTypes.Impostor;
    public override bool IsDead => false;
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.HackButton;

    public static CustomNumberOption HackCooldown;
    public static CustomNumberOption HackUses;
    public static CustomNumberOption MapCooldown;
    public static CustomNumberOption MapDuration;
    public static CustomOptionGroup Group;

    public void CreateOptions()
    {
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

        Group = new CustomOptionGroup($"{RoleColor.ToTextColor()}Hacker</color>",
            numberOpt: [HackCooldown, HackUses, MapCooldown, MapDuration],
            stringOpt: [],
            toggleOpt: [], role: typeof(HackerRole));
    }
}