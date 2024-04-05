using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using Reactor.Networking.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Networking;

public static class ScannerRpc
{
    [MethodRpc((uint)LaunchpadRpc.CreateScanner)]
    public static void RpcCreateScanner(this PlayerControl playerControl, float x, float y)
    {
        if (playerControl.Data.Role is not TrackerRole)
        {
            return;
        }
        
        var newScanner = ScannerManager.Instance.CreateScanner(playerControl, new Vector3(x, y, 0.0057f));
        ScannerManager.Instance.scanners.Add(newScanner);
    }
}