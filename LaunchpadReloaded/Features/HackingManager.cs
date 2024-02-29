using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using static ShipStatus;

namespace LaunchpadReloaded.Features;
public static class HackingManager
{
    public static List<byte> HackedPlayers = new List<byte>();
    public static List<HackNodeComponent> Nodes = new List<HackNodeComponent>();

    private static Dictionary<MapType, Vector3[]> MapNodePositions = new Dictionary<MapType, Vector3[]>
    {
        [MapType.Ship] = [ 
            new Vector3(-3.9285f, 5.6983f, 0.0057f),
            new Vector3(12.1729f, -6.5887f, -0.0066f),
            new Vector3(-19.7123f, -6.8006f, -0.0068f),
            new Vector3(-12.3633f, -14.6075f, -0.0146f) ]
    };

    public static bool AnyActiveNodes()
    {
        return Nodes.Count(node => node.IsActive) > 0;
    }

    [MethodRpc((uint)LaunchpadRPC.HackPlayer)]
    public static void RpcHackPlayer(PlayerControl player)
    {
        Debug.Log(player.Data.PlayerName + " is being hacked on local client.");
        HackedPlayers.Add(player.PlayerId);
        player.RawSetName("<b><i>???</b></i>");
        player.RawSetColor(15);
    }

    [MethodRpc((uint)LaunchpadRPC.UnhackPlayer)]
    public static void RpcUnhackPlayer(PlayerControl player)
    {
        HackedPlayers.Remove(player.PlayerId);
        player.SetName(player.Data.PlayerName);
        player.SetColor((byte)player.Data.DefaultOutfit.ColorId);
    }

    [MethodRpc((uint)LaunchpadRPC.CreateNodes)]
    public static void RpcCreateNodes(ShipStatus shipStatus)
    {
        GameObject nodesParent = new GameObject("Nodes");
        nodesParent.transform.SetParent(shipStatus.transform);

        Vector3[] nodePositions = MapNodePositions[shipStatus.Type];
        for (int i = 0; i < nodePositions.Length; i++)
        {
            Vector3 nodePos = nodePositions[i];
            CreateNode(shipStatus, i, nodesParent.transform, nodePos);
        }
    }

    [MethodRpc((uint)LaunchpadRPC.ToggleNode)]
    public static void RpcToggleNode(ShipStatus shipStatus, int nodeId, bool value)
    {
        HackNodeComponent node = Nodes.Find(node => node.Id == nodeId);
        Debug.Log(node.gameObject.transform.position.ToString());
        node.IsActive = value;  
    }

    public static HackNodeComponent CreateNode(ShipStatus shipStatus, int id, Transform parent, Vector3 position)
    {
        GameObject node = new GameObject("Node");
        node.transform.SetParent(parent, false);
        node.transform.localPosition = position;

        SpriteRenderer sprite = node.AddComponent<SpriteRenderer>();
        sprite.sprite = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Node.png");
        node.layer = LayerMask.NameToLayer("ShortObjects");
        sprite.transform.localScale = new Vector3(1, 1, 1);

        sprite.material = shipStatus.AllConsoles[0].gameObject.GetComponent<SpriteRenderer>().material;

        CircleCollider2D collider = node.AddComponent<CircleCollider2D>();
        collider.radius = 0.1082f;
        collider.offset = new Vector2(-0.01f, -0.3049f);

        HackNodeComponent nodeComponent = node.AddComponent<HackNodeComponent>();
        nodeComponent.Image = sprite;
        nodeComponent.Id = id;

        node.SetActive(true);
        Nodes.Add(nodeComponent);
        return nodeComponent;
    }
}
