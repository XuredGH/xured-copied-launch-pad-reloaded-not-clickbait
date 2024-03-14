using System.Text;
using AmongUs.GameOptions;
using BepInEx.Configuration;
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
    LoadableAsset<Sprite> Icon => LaunchpadAssets.NoImage;

    ConfigDefinition NumConfigDefinition => new("Roles", $"Num{RoleName}");

    ConfigDefinition ChanceConfigDefinition => new("Roles", $"Chance{RoleName}");

    bool AffectedByLight => Team == RoleTeamTypes.Crewmate;

    bool CanGetKilled => Team == RoleTeamTypes.Crewmate;

    bool CanUseKill => Team == RoleTeamTypes.Impostor;

    bool CanUseVent => Team == RoleTeamTypes.Impostor;

    bool TargetsBodies => false;
    bool CreateCustomTab => true;

    RoleTypes GhostRole => Team == RoleTeamTypes.Crewmate ? RoleTypes.CrewmateGhost : RoleTypes.ImpostorGhost;

    void PlayerControlFixedUpdate(PlayerControl playerControl) { }

    void HudUpdate(HudManager hudManager) { }
    StringBuilder SetTabText() { return null; }
}