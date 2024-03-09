using System;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Buttons;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class JanitorRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Janitor";
    public string RoleDescription => "Drag bodies and hide them in vents";
    public string RoleLongDescription => "The Janitor is an impostor with the ability to drag bodies! The Janitor can stash bodies in vents, disabling them in the process! Cleaning vents will expose the body!";
    public Color RoleColor => Color.yellow;
    public RoleTeamTypes Team => RoleTeamTypes.Impostor;
    public override bool IsDead => false;
    public bool TargetsBodies => true;
}