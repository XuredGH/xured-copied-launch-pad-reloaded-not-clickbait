using LaunchpadReloaded.API.Roles;
using Reactor.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class HackerRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Hacker";
    public string RoleDescription => "Hack meetings and sabotage the crewmates";
    public string RoleLongDescription => "The hacker can hack meetings which causes everyone to go anonymous, and can perform an advanced comms sabotage which causes everyone to go anonymous in game.";
    public Color RoleColor => new Color(20, 148, 20);
    public RoleTeamTypes Team => RoleTeamTypes.Impostor;
}