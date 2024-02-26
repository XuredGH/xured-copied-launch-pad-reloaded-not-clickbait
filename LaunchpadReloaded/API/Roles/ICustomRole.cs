using AmongUs.GameOptions;
using BepInEx.Configuration;
using Il2CppInterop.Runtime.Attributes;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.Roles;

public interface ICustomRole
{
    string RoleName { get; }
    string RoleDescription { get; }
    string RoleLongDescription { get; }
    Color RoleColor { get; }
    RoleTeamTypes Team { get; }
    ConfigDefinition NumConfigDefinition => new ("Roles",$"Num{RoleName}");
    ConfigDefinition ChanceConfigDefinition => new ("Roles",$"Chance{RoleName}");
    bool AffectedByLight => Team == RoleTeamTypes.Crewmate;
    bool CanGetKilled => Team == RoleTeamTypes.Crewmate;
    bool CanUseKill => Team == RoleTeamTypes.Impostor;
    bool CanUseVent => Team == RoleTeamTypes.Impostor;
    public RoleTypes GhostRole =>
        Team == RoleTeamTypes.Crewmate ? RoleTypes.CrewmateGhost : RoleTypes.ImpostorGhost;
    void PlayerControlFixedUpdate(PlayerControl playerControl) {}
    void HudUpdate(HudManager hudManager) {}
}