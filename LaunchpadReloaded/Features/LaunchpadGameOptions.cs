using AmongUs.Data;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.Features;

public class LaunchpadGameOptions
{
    private static LaunchpadGameOptions _instance;

    public static LaunchpadGameOptions Instance
    {
        get { return _instance ??= new LaunchpadGameOptions(); }
    }

    public readonly CustomStringOption GameModes;
    public readonly CustomToggleOption BanCheaters;

    /*    public readonly CustomStringOption VotingType;
        public readonly CustomNumberOption MaxVotes;*/

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
    public readonly CustomStringOption Character;
    public readonly CustomOptionGroup FunGroup;

    private LaunchpadGameOptions()
    {
        GameModes = new CustomStringOption("Gamemode", 0, ["Default", "Battle Royale"])
        {
            ChangedEvent = i =>
            {
                if (!AmongUsClient.Instance.AmHost)
                {
                    return;
                }
                CustomGameModeManager.RpcSetGameMode(GameData.Instance, i);
            }
        };

        /*        VotingType = new CustomStringOption("Voting Type", 0, ["Classic", "Chance", "Multiple", "Combined"]);
                VotingType.ChangedEvent = i => VotingTypesManager.RpcSetType(GameData.Instance, VotingType.IndexValue);

                MaxVotes = new CustomNumberOption("Max Votes", 3, 2, 10, 1, NumberSuffixes.None);
                MaxVotes.Hidden = () => !VotingTypesManager.CanVoteMultiple();*/

        BanCheaters = new CustomToggleOption("Ban Cheaters", true);
        
        DisableMeetingTeleport = new CustomToggleOption("Disable Meeting Teleport", false);
        OnlyShowRoleColor = new CustomToggleOption("Reveal Crewmate Roles", false);
        GeneralGroup = new CustomOptionGroup("General Options",
            toggleOpt: [OnlyShowRoleColor, DisableMeetingTeleport],
            stringOpt: [],
            numberOpt: []);

        FriendlyFire = new CustomToggleOption("Friendly Fire", false);
        UniqueColors = new CustomToggleOption("Unique Colors", true)
        {
            ChangedEvent = value =>
            {
                if (!AmongUsClient.Instance.AmHost || !value)
                {
                    return;
                }

                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (GradientManager.TryGetColor(player.PlayerId, out var grad))// && !player.AmOwner)
                    {
                        Rpc<CustomCheckColorRpc>.Instance.Handle(player, new CustomCheckColorRpc.Data((byte)player.Data.DefaultOutfit.ColorId, grad));
                    }
                }
            }
        };

        Character = new CustomStringOption("Character", 0, ["Default", "Horse", "Long"]);
        Character.ChangedEvent = i =>
        {
            PlayerBodyTypes bodyType;
            switch (Character.Value)
            {
                default:
                case "Default":
                    bodyType = PlayerBodyTypes.Normal;
                    break;
                case "Horse":
                    bodyType = PlayerBodyTypes.Horse;
                    break;
                case "Long":
                    bodyType = PlayerBodyTypes.Long;
                    break;
            }

            foreach (PlayerControl plr in PlayerControl.AllPlayerControls)
            {
                plr.MyPhysics.SetBodyType(bodyType);
                if (bodyType == PlayerBodyTypes.Normal) plr.cosmetics.currentBodySprite.BodySprite.transform.localScale = new(0.5f, 0.5f, 1f);
            }
        };

        FunGroup = new CustomOptionGroup("Fun Options",
            toggleOpt: [FriendlyFire, UniqueColors],
            stringOpt: [Character],
            numberOpt: []);

        SeekerCharacter = new CustomToggleOption("Use Seeker Character", true);
        ShowKnife = new CustomToggleOption("Show Knife", true)
        {
            Hidden = () => SeekerCharacter.Value == false
        };

        BattleRoyaleGroup = new CustomOptionGroup("Battle Royale Options",
            toggleOpt: [SeekerCharacter, ShowKnife],
            stringOpt: [],
            numberOpt: [])
        {
            Hidden = () => GameModes.IndexValue != (int)LaunchpadGamemodes.BattleRoyale
        };

        GeneralGroup.Hidden = FunGroup.Hidden = () => GameModes.Value != "Default";

        foreach (var role in CustomRoleManager.CustomRoles)
        {
            if (role.Value is ICustomRole customRole)
            {
                customRole.CreateOptions();
            }
        }
    }

    public static void Initialize()
    {
        _instance = new LaunchpadGameOptions();
    }
}