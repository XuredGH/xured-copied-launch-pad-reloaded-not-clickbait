using LaunchpadReloaded.Features.Managers;
using Reactor.Networking.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Networking;

public static class HackerRpc
{
    
    [MethodRpc((uint)LaunchpadRpc.CreateNodes)]
    public static void RpcCreateNodes(this ShipStatus shipStatus)
    {
        var nodesParent = new GameObject("Nodes");
        nodesParent.transform.SetParent(shipStatus.transform);

        var nodePositions = HackingManager.Instance.MapNodePositions[shipStatus.Type];
        if (shipStatus.TryCast<AirshipStatus>())
        {
            nodePositions = HackingManager.Instance.AirshipPositions;
        }

        for (var i = 0; i < nodePositions.Length; i++)
        {
            var nodePos = nodePositions[i];
            HackingManager.Instance.CreateNode(shipStatus, i, nodesParent.transform, nodePos);
        }
    }
}