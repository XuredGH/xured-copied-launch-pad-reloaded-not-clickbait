using AmongUs.GameOptions;
using BepInEx.Configuration;
using LaunchpadReloaded.Utilities;
using System.Text;
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
    bool IsNeutral => false;
    bool CanUseKill => Team == RoleTeamTypes.Impostor;
    bool CanUseVent => false;

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