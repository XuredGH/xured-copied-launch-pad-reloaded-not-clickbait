﻿using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Roles;
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

    public static AssetBundle Bundle;
    
    public override void Load()
    {
        Harmony.PatchAll();

        Bundle = AssetBundleManager.Load("assets");
        
        // TODO: CREATE ATTRIBUTE FOR THIS VVV
        CustomRoleManager.RegisterAllRoles();
        CustomButtonManager.RegisterAllButtons();
        Config.Save();
    }


}