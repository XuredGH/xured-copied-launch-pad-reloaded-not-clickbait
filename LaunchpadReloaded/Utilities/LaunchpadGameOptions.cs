using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using System;
using System.Collections.Generic;

namespace LaunchpadReloaded.Utilities;

public class LaunchpadGameOptions
{
    public static LaunchpadGameOptions Instance { get; private set; }

    public CustomStringOption GameModes;

    // General Options
    public CustomToggleOption OnlyShowRoleColor;
    public CustomOptionGroup GeneralGroup;

    // Battle Royale
    public CustomToggleOption SeekerCharacter;
    public CustomToggleOption ShowKnife;
    public CustomOptionGroup BattleRoyaleGroup;

    // Fun Options
    public CustomToggleOption FriendlyFire;
    public CustomOptionGroup FunGroup;

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

        OnlyShowRoleColor = new CustomToggleOption("Reveal Crewmate Roles", false);
        GeneralGroup = new CustomOptionGroup("General Options",
            toggleOpt: [OnlyShowRoleColor],
            stringOpt: [],
            numberOpt: []);

        FriendlyFire = new CustomToggleOption("Friendly Fire", false);
        FunGroup = new CustomOptionGroup("Fun Options",
            toggleOpt: [FriendlyFire],
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