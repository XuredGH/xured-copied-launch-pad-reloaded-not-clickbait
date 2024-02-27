﻿using AmongUs.GameOptions;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using LaunchpadReloaded.API.Options;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Misc.CustomOptions;
using LaunchpadReloaded.Roles;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Extensions;

namespace LaunchpadReloaded;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class LaunchpadReloadedPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        Harmony.PatchAll();
        CustomRoleManager.RegisterRole(typeof(CaptainRole));
        CustomRoleManager.RegisterRole(typeof(JanitorRole));
        CustomOptionsManager.RegisterCustomOption(typeof(SabotageShake));
        CustomOptionsManager.RegisterCustomOption(typeof(Test));
        Config.Save();
    }
}