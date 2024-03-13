﻿using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Components;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class LaunchpadReloadedPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);
    public static LaunchpadReloadedPlugin Instance { get; private set; }

    public static AssetBundle Bundle;
    public static Material Mat;
    public static Sprite BlankButton;
    
    public override void Load()
    {
        Instance = this;
        Harmony.PatchAll();

        Bundle = AssetBundleManager.Load("assets");

        Mat = Bundle.LoadAsset<Material>("GradientPlayerMaterial").DontUnload();
        BlankButton = Bundle.LoadAsset<Sprite>("BlankButton").DontUnload();
        
        // TODO: CREATE ATTRIBUTE FOR THIS VVV
        CustomGamemodeManager.RegisterAllGamemodes();
        CustomGamemodeManager.SetGamemode(0);
        CustomRoleManager.RegisterAllRoles();
        CustomButtonManager.RegisterAllButtons();

        new LaunchpadGameOptions();
        
        Config.Save();
    }
    
}