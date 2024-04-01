using System.Linq;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Networking;

public static class HackerRpc
{
    
    [MethodRpc((uint)LaunchpadRpc.HackPlayer)]
    public static void RpcHackPlayer(this PlayerControl source, PlayerControl target)
    {
        if (source.Data.Role is not HackerRole)
        {
            return;
        }
        
        HackingManager.Instance.hackedPlayers.Add(target.PlayerId);
        HackingManager.HackPlayer(target);
        
        foreach (var data in GameData.Instance.AllPlayers.ToArray().Where(x => x.Role is HackerRole))
        {
            HackingManager.HackPlayer(data.Object);
        }
        
        if (!target.AmOwner)
        {
            return;
        }
        
        Coroutines.Start(HackingManager.HackEffect());   
        foreach (var node in HackingManager.Instance.nodes)
        {
            HackingManager.ToggleNode(node.Id, true);
        }
    }

    [MethodRpc((uint)LaunchpadRpc.UnHackPlayer)]
    public static void RpcUnHackPlayer(this PlayerControl player)
    { 
        HackingManager.Instance.hackedPlayers.Remove(player.PlayerId);
        HackingManager.UnHackPlayer(player);

        if (!HackingManager.Instance.AnyPlayerHacked())
        {
            foreach (var data in GameData.Instance.AllPlayers.ToArray().Where(x => x.Role is HackerRole))
            {
                HackingManager.UnHackPlayer(data.Object);
            }
        }
        
        if (!player.AmOwner)
        {
            return;
        }
        
        Coroutines.Stop(HackingManager.HackEffect());
        foreach (var node in HackingManager.Instance.nodes)
        {
            HackingManager.ToggleNode(node.Id, false);
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