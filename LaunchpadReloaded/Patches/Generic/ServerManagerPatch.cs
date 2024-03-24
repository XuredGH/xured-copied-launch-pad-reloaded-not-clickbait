using HarmonyLib;

namespace LaunchpadReloaded.Patches.Generic;

/// <summary>
/// Add Launchpad Server
/// </summary>
//[HarmonyPatch(typeof(ServerManager), nameof(ServerManager.LoadServers))]
public static class ServerManagerPatch
{
    public static void Postfix(ServerManager __instance)
    {
        var serverInfo = new ServerInfo("http-1", "http://" + LaunchpadReloadedPlugin.LaunchpadServerAddress, LaunchpadReloadedPlugin.LaunchpadServerPort, false);
        ServerInfo[] arr = [serverInfo];
        var regionInfo = new StaticHttpRegionInfo("Launchpad Beta Testing", (StringNames)1003, LaunchpadReloadedPlugin.LaunchpadServerAddress, arr);

        __instance.AddOrUpdateRegion(regionInfo.Cast<IRegionInfo>());
    }
}