using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using System;
using System.Collections.Generic;

namespace LaunchpadReloaded.Utilities;

public class LaunchpadGameOptions
{
    public static LaunchpadGameOptions Instance { get; private set; }

    // General Options
    public CustomToggleOption FriendlyFire;
    public CustomStringOption GameModes;
    public CustomOptionGroup GeneralGroup;

    // Battle Royale
    public CustomToggleOption SeekerCharacter;
    public CustomToggleOption ShowKnife;
    public CustomOptionGroup BattleRoyaleGroup;

    public LaunchpadGameOptions()
    {
        if (Instance != null)
        {
            throw new Exception("Can't have more than one Launchpad Options");
        }

        GameModes = new CustomStringOption("GameModes", 0, ["Default", "Battle Royale"]);
        GameModes.ChangedEvent = i =>
        {
            if (AmongUsClient.Instance.AmHost)
            {
                CustomGameModeManager.RpcSetGameMode(PlayerControl.LocalPlayer, i);
            }
        };

        FriendlyFire = new CustomToggleOption("Friendly Fire", false);

        GeneralGroup = new CustomOptionGroup("General Options",
            toggleOpt: [FriendlyFire],
            stringOpt: [GameModes,],
            numberOpt: []);

        SeekerCharacter = new CustomToggleOption("Use Seeker Character", true);
        ShowKnife = new CustomToggleOption("Show Knife", true);

        BattleRoyaleGroup = new CustomOptionGroup("Battle Royale Options",
            toggleOpt: [SeekerCharacter, ShowKnife],
            stringOpt: [],
            numberOpt: []);
        BattleRoyaleGroup.Hidden = () => GameModes.Value != "Battle Royale";

        foreach (KeyValuePair<ushort, RoleBehaviour> role in CustomRoleManager.CustomRoles)
        {
            ICustomRole customRole = role.Value as ICustomRole;
            if (customRole != null) customRole.CreateOptions();
        }

        Instance = this;
    }
}