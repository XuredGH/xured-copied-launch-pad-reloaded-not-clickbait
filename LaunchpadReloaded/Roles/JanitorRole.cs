using System;
using System.Text;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class JanitorRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Janitor";
    public string RoleDescription => "Drag bodies and hide them in vents";
    public string RoleLongDescription => "You can drag bodies and hide them in vents\nWhich will cause them to disappear unless the vent is used.";
    public Color RoleColor => Color.yellow;
    public RoleTeamTypes Team => RoleTeamTypes.Impostor;
    public bool TargetsBodies => true;

    public StringBuilder SetTabText()
    {
        StringBuilder taskStringBuilder = Helpers.CreateForRole(this);
        return taskStringBuilder;
    }
}