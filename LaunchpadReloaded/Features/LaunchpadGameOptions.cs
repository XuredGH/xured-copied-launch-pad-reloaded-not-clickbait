using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using System;

namespace LaunchpadReloaded.Features;

public class LaunchpadGameOptions
{
    public static LaunchpadGameOptions Instance { get; private set; }

    public readonly CustomStringOption GameModes;

    // General Options
    public readonly CustomToggleOption OnlyShowRoleColor;
    public readonly CustomToggleOption DisableMeetingTeleport;
    public readonly CustomOptionGroup GeneralGroup;

    // Battle Royale
    public readonly CustomToggleOption SeekerCharacter;
    public readonly CustomToggleOption ShowKnife;
    public readonly CustomOptionGroup BattleRoyaleGroup;

    // Fun Options
    public readonly CustomToggleOption FriendlyFire;
    public readonly CustomToggleOption UniqueColors;
    public readonly CustomOptionGroup FunGroup;

    public LaunchpadGameOptions()
    {
        if (Instance != null)
        {
            throw new Exception("Can't have more than one Launchpad Options");
        }

        GameModes = new CustomStringOption("Gamemode", 0, ["Default", "Battle Royale"]);
        GameModes.ChangedEvent = i =>
        {
            if (AmongUsClient.Instance.AmHost)
            {
                CustomGameModeManager.RpcSetGameMode(PlayerControl.LocalPlayer, i);
            }
        };

        DisableMeetingTeleport = new CustomToggleOption("Disable Meeting Teleport", false);
        OnlyShowRoleColor = new CustomToggleOption("Reveal Crewmate Roles", false);
        GeneralGroup = new CustomOptionGroup("General Options",
            toggleOpt: [OnlyShowRoleColor, DisableMeetingTeleport],
            stringOpt: [],
            numberOpt: []);

        FriendlyFire = new CustomToggleOption("Friendly Fire", false);
        UniqueColors = new CustomToggleOption("Unique Colors", true);
        UniqueColors.ChangedEvent = i =>
        {
            if (!AmongUsClient.Instance.AmHost || i == false) return;
            foreach (var plr in PlayerControl.AllPlayerControls)
            {
                plr.CmdCheckColor((byte)plr.cosmetics.ColorId);
            }
        };

        FunGroup = new CustomOptionGroup("Fun Options",
            toggleOpt: [FriendlyFire, UniqueColors],
            stringOpt: [],
            numberOpt: []);

        SeekerCharacter = new CustomToggleOption("Use Seeker Character", true);
        ShowKnife = new CustomToggleOption("Show Knife", true);
        ShowKnife.Hidden = () => SeekerCharacter.Value == false;

        BattleRoyaleGroup = new CustomOptionGroup("Battle Royale Options",
            toggleOpt: [SeekerCharacter, ShowKnife],
            stringOpt: [],
            numberOpt: []);

        BattleRoyaleGroup.Hidden = () => GameModes.Value != "Battle Royale";
        GeneralGroup.Hidden = FunGroup.Hidden = () => GameModes.Value != "Default";

        foreach (var role in CustomRoleManager.CustomRoles)
        {
            var customRole = role.Value as ICustomRole;
            if (customRole != null)
            {
                customRole.CreateOptions();
            }
        }

        Instance = this;
    }
}