using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using Il2CppSystem.Text;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class JesterRole(System.IntPtr ptr) : RoleBehaviour(ptr), ICustomRole
{
    public string RoleName => "Jester";
    public ushort RoleId => (ushort)LaunchpadRoles.Jester;
    public string RoleDescription => "Get ejected to win";
    public string RoleLongDescription => "Convince the crew to vote you out by being suspicious.\nIf you get voted out, you win the game.";
    public Color RoleColor => LaunchpadPalette.JesterColor;
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public bool IsOutcast => true;
    public bool TasksCount => false;
    public bool CanUseVent => CanUseVents?.Value ?? true;
    public RoleTypes GhostRole => (RoleTypes)LaunchpadRoles.OutcastGhost;
    public override bool IsDead => false;
    
    [HideFromIl2Cpp]
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.JesterIcon;

    public override void AppendTaskHint(StringBuilder taskStringBuilder) { }
    public override bool DidWin(GameOverReason reason)
    {
        return reason == (GameOverReason)GameOverReasons.JesterWins;
    }

    public string GetCustomEjectionMessage(GameData.PlayerInfo exiled)
    {
        return $"You've been fooled! {exiled.PlayerName} was The Jester.";
    }
    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, this.Player))
        {
            return false;
        }

        Console console = usable.TryCast<Console>();
        return !(console != null) || console.AllowImpostor;
    }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer) return;
        ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl, 0);
        orCreateTask.Text = string.Concat(new string[]
            {
                LaunchpadPalette.JesterColor.ToTextColor(),
                DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FakeTasks, Il2CppSystem.Array.Empty<Il2CppSystem.Object>()),
                "</color>"
            });

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