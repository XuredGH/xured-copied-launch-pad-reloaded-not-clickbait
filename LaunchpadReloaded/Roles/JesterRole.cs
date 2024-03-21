using Il2CppSystem.Text;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class JesterRole(IntPtr ptr) : RoleBehaviour(ptr), ICustomRole
{
    public string RoleName => "Jester";
    public ushort RoleId => (ushort)LaunchpadRoles.Jester;
    public string RoleDescription => "Get ejected to win";
    public string RoleLongDescription => "Convince the crew to vote you out!";
    public Color RoleColor => LaunchpadPalette.JesterColor;
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public bool IsOutcast => true;
    public bool TasksCount => false;
    public bool CanUseVent => CanUseVents is not null ? CanUseVents.Value : true;
    public override bool IsDead => false;
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.ZoomButton;

    public override void AppendTaskHint(StringBuilder taskStringBuilder) { }
    public override bool DidWin(GameOverReason reason)
    {
        return reason == (GameOverReason)CustomGameOverReason.JesterWins;
    }

    public string GetCustomEjectionMessage(GameData.PlayerInfo exiled)
    {
        return $"You've been fooled! {exiled.PlayerName} was The Jester.";
    }

    public static CustomToggleOption CanUseVents;
    public static CustomOptionGroup Group;
    public void CreateOptions()
    {
        CanUseVents = new("Can Use Vents", true, typeof(JesterRole));
        Group = new CustomOptionGroup($"{RoleColor.ToTextColor()}Jester</color>",
            numberOpt: [],
            stringOpt: [],
            toggleOpt: [CanUseVents], role: typeof(JesterRole));
    }
}