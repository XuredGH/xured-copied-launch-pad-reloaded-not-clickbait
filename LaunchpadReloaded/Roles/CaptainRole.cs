using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using System;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
[RegisterCustomRole((ushort)LaunchpadRoles.Captain)]
public class CaptainRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Captain";
    
    public string RoleDescription => "Protect the crew with your abilities";
    
    public string RoleLongDescription => "Use your zoom ability to keep an eye on the crew\n And call meetings from any location!";
    
    public Color RoleColor => LaunchpadPalette.CaptainColor;
    
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;

    public LoadableAsset<Sprite> OptionsScreenshot => MiraAssets.Empty;

    public override bool IsDead => false;
    
    [HideFromIl2Cpp]
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.ZoomButton;
}