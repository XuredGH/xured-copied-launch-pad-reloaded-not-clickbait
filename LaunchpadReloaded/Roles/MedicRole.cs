﻿using System;
using LaunchpadReloaded.Features;
using MiraAPI.Roles;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;


[RegisterCustomRole]
public class MedicRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Medic";
    public string RoleDescription => "Help the crewmates by reviving dead players.";
    public string RoleLongDescription => "Use your revive ability to bring dead bodies\nback to life!";
    public Color RoleColor => LaunchpadPalette.MedicColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.ReviveButton,
    };
}
