﻿using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Utilities;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;

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

        
        // TODO: CREATE ATTRIBUTE FOR THIS VVV
        CustomGamemodeManager.RegisterAllGamemodes();
        CustomGamemodeManager.SetGamemode(0);
        CustomRoleManager.RegisterAllRoles();
        CustomButtonManager.RegisterAllButtons();

        new LaunchpadGameOptions();
        
        Config.Save();
    }
    
}