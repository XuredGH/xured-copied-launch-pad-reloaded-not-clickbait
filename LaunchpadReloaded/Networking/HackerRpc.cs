using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using Reactor.Networking.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Networking;

public static class HackerRpc
{
    [MethodRpc((uint)LaunchpadRpc.UnhackPlayer)]
    public static void RpcUnHackPlayer(this PlayerControl player)
    {
        HackingManager.UnHackPlayer(player);
    }

    [MethodRpc((uint)LaunchpadRpc.ToggleNode)]
    public static void RpcToggleNode(this PlayerControl sender, int nodeId, bool value)
    {
        if (value == true && sender.Data.Role is not HackerRole)
        {
            sender.KickForCheating();
            return;
        }

        HackNodeComponent node = HackingManager.Instance.nodes[nodeId];
        if (node == null || node.isActive == value) return;

        HackingManager.ToggleNode(nodeId, value);

        if (value)
        {
            foreach (PlayerControl plr in PlayerControl.AllPlayerControls) HackingManager.HackPlayer(plr);
        }
    }

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