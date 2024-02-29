using System;
using LaunchpadReloaded.API.Roles;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class JanitorRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Janitor";
    public string RoleDescription => "Help the impostor by\ndragging and hiding bodies";
    public string RoleLongDescription => "Help the Impostor with your ability to move bodies around and bury them!";
    public Color RoleColor => Color.yellow;
    public RoleTeamTypes Team => RoleTeamTypes.Impostor;
    
}