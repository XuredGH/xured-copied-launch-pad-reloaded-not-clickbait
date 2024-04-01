using System;
using System.Linq;
using System.Text;
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
using Reactor.Patches;
using TMPro;

namespace LaunchpadReloaded;

[BepInAutoPlugin("dev.xtracube.launchpad","LaunchpadReloaded")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class LaunchpadReloadedPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);
    public static LaunchpadReloadedPlugin Instance { get; private set; }

    public override void Load()
    {
        Instance = this;
        Harmony.PatchAll();

        RegisterColors();

        CustomGameModeManager.RegisterAllGameModes();
        CustomGameModeManager.SetGameMode(0);
        CustomRoleManager.RegisterAllRoles();
        CustomButtonManager.RegisterAllButtons();

        LaunchpadGameOptions.Initialize();
        LaunchpadSettings.Initialize();

        ReactorVersionShower.TextUpdated += VersionShower;

        Config.Save();
    }

    private static void VersionShower(TextMeshPro textMeshPro)
    {
        textMeshPro.text = new StringBuilder("<color=#FF4050FF>Launchpad</color> ")
            .Append(GetShortHashVersion(Version))
            .Append("\nPowered by <color=#FFB793>CrowdedMod</color>\n& <color=#348feb>Mini.RegionInstall</color>\n")
            .Append(textMeshPro.text)
            .ToString();
    }

    private static string GetShortHashVersion(string version)
    {
        var index = version.IndexOf("+", StringComparison.Ordinal);

        return index < 0 ? version : version[..(index + 8)];
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