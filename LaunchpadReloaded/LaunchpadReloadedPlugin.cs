using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Colors;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using System.Linq;
using Reactor.Patches;
using TMPro;

namespace LaunchpadReloaded;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class LaunchpadReloadedPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);
    public static LaunchpadReloadedPlugin Instance { get; private set; }

    public const string LaunchpadServerAddress = "brand-lauderdale.gl.at.ply.gg";
    public const ushort LaunchpadServerPort = 7764;

    public override void Load()
    {
        Instance = this;
        Harmony.PatchAll();

        RegisterColors();

        CustomGameModeManager.RegisterAllGameModes();
        CustomGameModeManager.SetGameMode(0);
        CustomRoleManager.RegisterAllRoles();
        CustomButtonManager.RegisterAllButtons();

        new LaunchpadGameOptions();


        ReactorVersionShower.TextUpdated += VersionShower;

        Config.Save();
    }

    private static void VersionShower(TextMeshPro textMeshPro)
    {
        textMeshPro.text = $"Launchpad {Version[..13]}\n{textMeshPro.text}";
    }
    
    private static void RegisterColors()
    {
        var colors =
            typeof(LaunchpadColors)
            .GetProperties()
            .Select(s => (CustomColor)s.GetValue(null))
            .ToArray();

        Palette.PlayerColors = Palette.PlayerColors.ToArray().AddRangeToArray(colors.Select(x => x.MainColor).ToArray());
        Palette.ShadowColors = Palette.ShadowColors.ToArray().AddRangeToArray(colors.Select(x => x.ShadowColor).ToArray());
        Palette.ColorNames = Palette.ColorNames.ToArray().AddRangeToArray(colors.Select(x => x.Name).ToArray());
    }
}