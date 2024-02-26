using System;
using LaunchpadReloaded.API.Roles;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class CaptainRole : CrewmateRole, ICustomRole
{
    public string RoleName => "Captain";
    public string RoleDescription => "Protect the crew with your abilities";
    public string RoleLongDescription => "Use your zoom ability to keep an eye on the crew and call a meeting from any location!";
    public Color RoleColor => Color.gray;
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public override bool IsDead => false;
    
    public CaptainRole()
    {
        Debug.LogError("CAPTAIN INIT");
    }

    public CaptainRole(IntPtr ptr) : base(ptr)
    {
    }
}