﻿using System;
using System.Text;
using AmongUs.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class CaptainRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Captain";
    public ushort RoleId => (ushort)LaunchpadRoles.Captain;
    public string RoleDescription => "Protect the crew with your abilities";
    public string RoleLongDescription => "Use your zoom ability to keep an eye on the crew\n And call meetings from any location!";
    public Color RoleColor => LaunchpadPalette.CaptainColor;
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public override bool IsDead => false;
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.ZoomButton;

    public StringBuilder SetTabText()
    {
        var taskStringBuilder = Helpers.CreateForRole(this);
        return taskStringBuilder;
    }
}