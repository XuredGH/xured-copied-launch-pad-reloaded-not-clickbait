using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class HackerRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Hacker";
    public string RoleDescription => "Hack meetings and sabotage the crewmates";
    public string RoleLongDescription => "Hack crewmates and make them unable to do tasks\nAnd view the admin map from anywhere!";
    public Color RoleColor => LaunchpadPalette.HackerColor;
    public RoleTeamTypes Team => RoleTeamTypes.Impostor;
    public override bool IsDead => false;
    public Sprite Icon => LaunchpadAssets.HackButton;
    public StringBuilder SetTabText()
    {
        StringBuilder taskStringBuilder = Helpers.CreateForRole(this);
        return taskStringBuilder;
    }
}