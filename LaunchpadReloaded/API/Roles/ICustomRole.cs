using System.Text;
using AmongUs.GameOptions;
using BepInEx.Configuration;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.Roles;

public interface ICustomRole
{
    string RoleName { get; }

    ushort RoleId { get; }

    string RoleDescription { get; }

    string RoleLongDescription { get; }

    Color RoleColor { get; }

    RoleTeamTypes Team { get; }

    LoadableAsset<Sprite> Icon => LaunchpadAssets.NoImage;

    ConfigDefinition NumConfigDefinition => new("Roles", $"Num{RoleName}");

    ConfigDefinition ChanceConfigDefinition => new("Roles", $"Chance{RoleName}");

    bool AffectedByLight => Team == RoleTeamTypes.Crewmate;

    bool CanGetKilled => Team == RoleTeamTypes.Crewmate;
    bool IsOutcast => false;
    bool CanKill => Team == RoleTeamTypes.Impostor;
    bool CanUseVent => Team == RoleTeamTypes.Impostor;
    bool TasksCount => Team == RoleTeamTypes.Crewmate;

    bool TargetsBodies => false;
    bool CreateCustomTab => true;

    RoleTypes GhostRole => Team == RoleTeamTypes.Crewmate ? RoleTypes.CrewmateGhost : RoleTypes.ImpostorGhost;

    void CreateOptions() { }
    void PlayerControlFixedUpdate(PlayerControl playerControl) { }

    void HudUpdate(HudManager hudManager) { }
    string GetCustomEjectionMessage(GameData.PlayerInfo player)
    {
        return Team == RoleTeamTypes.Impostor ? $"{player.PlayerName} was The {RoleName}" : null;
    }

    StringBuilder SetTabText()
    {
        var taskStringBuilder = Helpers.CreateForRole(this);
        return taskStringBuilder;
    }
}