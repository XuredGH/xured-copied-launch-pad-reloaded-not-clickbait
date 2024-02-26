using AmongUs.GameOptions;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Buttons;
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
        
        // TODO: CREATE ATTRIBUTE FOR THIS VVV
        CustomRoleManager.RegisterRole(typeof(CaptainRole));
        CustomRoleManager.RegisterRole(typeof(JanitorRole));
        CustomButtonManager.RegisterButton(typeof(CallButton));
        CustomButtonManager.RegisterButton(typeof(ZoomButton));
        CustomButtonManager.RegisterButton(typeof(CleanButton));
        Config.Save();
    }


}