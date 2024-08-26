using System;
using Il2CppInterop.Runtime.Attributes;
using LaunchpadReloaded.Features;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
[RegisterCustomRole]
public class MedicRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Medic";
    public string RoleDescription => "Help the crewmates by reviving dead players.";
    public string RoleLongDescription => "Use your revive ability to bring dead bodies\nback to life!";
    public Color RoleColor => LaunchpadPalette.MedicColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public override bool IsDead => false;
    public bool TargetsBodies => true;
    
    [HideFromIl2Cpp]
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.ReviveButton;
}