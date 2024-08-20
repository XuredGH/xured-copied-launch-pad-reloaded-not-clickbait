using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using System;
using MiraAPI.Roles;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
[RegisterCustomRole((ushort)LaunchpadRoles.Mayor)]
public class MayorRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Mayor";
    public string RoleDescription => "You get extra votes.";
    public string RoleLongDescription => "You get extra votes every round.\nUse these votes to eject the Impostor!";
    public Color RoleColor => LaunchpadPalette.MayorColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public override bool IsDead => false;

}