using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
[RegisterCustomRole]
public class MayorRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Mayor";
    public ushort RoleId => (ushort)LaunchpadRoles.Mayor;
    public string RoleDescription => "You get extra votes.";
    public string RoleLongDescription => "You get extra votes every round.\nUse these votes to eject the Impostor!";
    public Color RoleColor => LaunchpadPalette.MayorColor;
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public override bool IsDead => false;

    public static CustomNumberOption ExtraVotes;
    public static CustomOptionGroup Group;

    public void CreateOptions()
    {
        ExtraVotes = new CustomNumberOption("Extra Votes", 1, 1, 3, 1, NumberSuffixes.None, role: typeof(MayorRole));

        Group = new CustomOptionGroup($"{RoleColor.ToTextColor()}Mayor</color>",
            numberOpt: [ExtraVotes],
            stringOpt: [],
            toggleOpt: [], role: typeof(MayorRole));
    }

}