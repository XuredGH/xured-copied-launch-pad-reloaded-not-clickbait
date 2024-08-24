using System;
using Il2CppInterop.Runtime.Attributes;
using LaunchpadReloaded.Features;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
[RegisterCustomRole((ushort)LaunchpadRoles.Hacker)]
public class HackerRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Hacker";
    public string RoleDescription => "Hack meetings and sabotage the crewmates";
    public string RoleLongDescription => "Hack crewmates and make them unable to do tasks\nAnd view the admin map from anywhere!";
    public Color RoleColor => LaunchpadPalette.HackerColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public override bool IsDead => false;
    
    [HideFromIl2Cpp]
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.HackButton;

}