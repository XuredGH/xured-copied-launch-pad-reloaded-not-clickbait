using HarmonyLib;
using Reactor.Localization.Utilities;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(ServerManager),nameof(ServerManager.LoadServers))]
public static class ServerManagerPatch
{
    public static void Postfix(ServerManager __instance)
    {
        var serverInfo = new ServerInfo("http-1", "http://brand-lauderdale.gl.at.ply.gg", 7764, false);
        ServerInfo[] arr = [serverInfo];
        var regionInfo = new StaticHttpRegionInfo("launchpad test server", (StringNames)1003,"brand-lauderdale.gl.at.ply.gg", arr);
        
        __instance.AddOrUpdateRegion(regionInfo.Cast<IRegionInfo>());
        
        
    }
}