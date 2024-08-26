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
public class DetectiveRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Detective";
    public string RoleDescription => "Investigate and find clues on murders.";
    public string RoleLongDescription => "Investigate bodies to get clues and use your instinct ability\nto see recent footsteps around you!";
    public Color RoleColor => LaunchpadPalette.DetectiveColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate; 
    public override bool IsDead => false;
    public bool TargetsBodies => true;
    
    [HideFromIl2Cpp]
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.ZoomButton;
}