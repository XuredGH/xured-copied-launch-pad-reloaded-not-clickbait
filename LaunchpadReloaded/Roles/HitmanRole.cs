using System;
using LaunchpadReloaded.API.Roles;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class HitmanRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Hitman";
    public string RoleDescription => "Drag bodies and hide them in vents";
    public string RoleLongDescription => "The Hitman is an impostor with the ability to drag bodies! The Hitman can stash bodies in vents, disabling them in the process! Cleaning vents will expose the body!";
    public Color RoleColor => new Color(110, 0, 0);
    public RoleTeamTypes Team => RoleTeamTypes.Impostor;
}