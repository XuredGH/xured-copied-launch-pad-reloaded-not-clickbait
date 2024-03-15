using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ShipStatus;

namespace LaunchpadReloaded.Features;
[RegisterInIl2Cpp]
public class HackingManager(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static HackingManager? Instance;
    public List<byte> HackedPlayers = new();
    public List<HackNodeComponent> Nodes = new();

    private Dictionary<MapType, Vector3[]> MapNodePositions = new()
    {
        [MapType.Ship] = [
            new Vector3(-3.9285f, 5.6983f, 0.0057f),
            new Vector3(12.1729f, -6.5887f, -0.0066f),
            new Vector3(-19.7123f, -6.8006f, -0.0068f),
            new Vector3(-12.3633f, -14.6075f, -0.0146f) ],
        [MapType.Pb] = [
            new Vector3(3.5599f, -7.584f, -0.0076f),
            new Vector3(22.1169f, -25.0981f, -0.0251f),
            new Vector3(37.3687f, -21.9697f, -0.022f),
            new Vector3(40.6573f, -7.9562f, -0.008f) ],
        [MapType.Hq] = [
            new Vector3(11.5355f, 10.3573f, 0.0104f),
            new Vector3(-3.063f, 3.8147f, 0.0038f),
            new Vector3(16.6542f, 25.3223f, 0.0253f),
            new Vector3(19.5728f, 17.4778f, 0.0175f) ],
        [MapType.Fungle] = [
            new Vector3(-22.4063f, -1.6647f, -0.0017f),
            new Vector3(-11.0019f, 12.6502f, 0.0127f),
            new Vector3(24.3133f, 14.628f, 0.0146f),
            new Vector3(7.6678f, -9.9008f, -0.0099f)
            ],

        // Submerged compatibility soon
        [(MapType)6] = [
            ]
    };

    private Vector3[] AirshipPositions = [
        new Vector3(-5.0792f, 10.9539f, 0.011f),
        new Vector3(16.856f, 14.7769f, 0.0148f),
        new Vector3(37.3283f, -3.7612f, -0.0038f),
        new Vector3(19.8862f, -3.9247f, -0.0039f),
        new Vector3(-13.1688f, -14.4867f, -0.0145f),
        new Vector3(-14.2747f, -4.8171f, -0.0048f),
        new Vector3(1.4743f, -2.5041f, -0.0025f),
    ];

    private void Awake()
    {
        Instance = this;
    }

    public bool AnyActiveNodes()
    {
        return Nodes.Count(node => node.IsActive) > 0;
    }

    [MethodRpc((uint)LaunchpadRPC.HackPlayer)]
    public static void RpcHackPlayer(PlayerControl player)
    {
        Debug.Log(player.Data.PlayerName + " is being hacked on local client.");
        Instance.HackedPlayers.Add(player.PlayerId);
        player.RawSetName("<b><i>???</b></i>");
        player.RawSetColor(15);
    }

    [MethodRpc((uint)LaunchpadRPC.UnhackPlayer)]
    public static void RpcUnhackPlayer(PlayerControl player)
    {
        Instance.HackedPlayers.Remove(player.PlayerId);
        player.SetName(player.Data.PlayerName);
        player.SetColor((byte)player.Data.DefaultOutfit.ColorId);
        player.cosmetics.gameObject.SetActive(true);
    }

    [MethodRpc((uint)LaunchpadRPC.CreateNodes)]
    public static void RpcCreateNodes(ShipStatus shipStatus)
    {
        var nodesParent = new GameObject("Nodes");
        nodesParent.transform.SetParent(shipStatus.transform);

        var nodePositions = Instance.MapNodePositions[shipStatus.Type];
        if (shipStatus.TryCast<AirshipStatus>()) nodePositions = Instance.AirshipPositions;

        for (var i = 0; i < nodePositions.Length; i++)
        {
            var nodePos = nodePositions[i];
            Instance.CreateNode(shipStatus, i, nodesParent.transform, nodePos);
        }
    }

    [MethodRpc((uint)LaunchpadRPC.ToggleNode)]
    public static void RpcToggleNode(ShipStatus shipStatus, int nodeId, bool value)
    {
        var node = Instance.Nodes.Find(node => node.Id == nodeId);
        Debug.Log(node.gameObject.transform.position.ToString());
        node.IsActive = value;
        IEnumerable<GameData.PlayerInfo> hacker = GameData.Instance.AllPlayers.ToArray().Where(player => player.Role is HackerRole);
        foreach (GameData.PlayerInfo player in hacker)
        {
            player.Object.SetName(player.PlayerName);

            if (!value) player.Object.SetColor((byte)player.DefaultOutfit.ColorId);
            else player.Object.RawSetColor(15);

            player.Object.cosmetics.gameObject.SetActive(!value);
        }
    }

    public HackNodeComponent CreateNode(ShipStatus shipStatus, int id, Transform parent, Vector3 position)
    {
        var node = new GameObject("Node");
        node.transform.SetParent(parent, false);
        node.transform.localPosition = position;

        var sprite = node.AddComponent<SpriteRenderer>();
        sprite.sprite = LaunchpadAssets.NodeSprite.LoadAsset();
        node.layer = LayerMask.NameToLayer("ShortObjects");
        sprite.transform.localScale = new Vector3(1, 1, 1);

        sprite.material = shipStatus.AllConsoles[0].gameObject.GetComponent<SpriteRenderer>().material;

        var collider = node.AddComponent<CircleCollider2D>();
        collider.radius = 0.1082f;
        collider.offset = new Vector2(-0.01f, -0.3049f);

        var nodeComponent = node.AddComponent<HackNodeComponent>();
        nodeComponent.Image = sprite;
        nodeComponent.Id = id;

        node.SetActive(true);
        Nodes.Add(nodeComponent);
        return nodeComponent;
    }
}
