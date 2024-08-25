using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Colors;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using System.Linq;
using BepInEx.Configuration;
using MiraAPI;
using MiraAPI.PluginLoading;
using Reactor.Utilities;

namespace LaunchpadReloaded;

[BepInAutoPlugin("dev.xtracube.launchpad", "LaunchpadReloaded")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency(MiraApiPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class LaunchpadReloadedPlugin : BasePlugin, IMiraPlugin
{
    public Harmony Harmony { get; } = new(Id);
    public static LaunchpadReloadedPlugin Instance { get; private set; }

    public ConfigFile GetConfigFile()
    {
        return Config;
    }

    public override void Load()
    {
        Instance = this;
        Harmony.PatchAll();
        
        ReactorCredits.Register("Launchpad", Version[..11], true, ReactorCredits.AlwaysShow);

        RegisterColors();
        
        LaunchpadSettings.Initialize();

        Config.Save();
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