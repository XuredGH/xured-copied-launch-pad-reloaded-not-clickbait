using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;
using Random = System.Random;

namespace LaunchpadReloaded.Features.Managers;

[RegisterInIl2Cpp]
public class HackingManager(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static HackingManager Instance { get; private set; }

    public List<byte> hackedPlayers;
    public List<HackNodeComponent> nodes;

    private readonly Dictionary<ShipStatus.MapType, Vector3[]> _mapNodePositions = new()
    {
        [ShipStatus.MapType.Ship] = [
            new Vector3(-3.9285f, 5.6983f, 0.0057f),
            new Vector3(12.1729f, -6.5887f, -0.0066f),
            new Vector3(-19.7123f, -6.8006f, -0.0068f),
            new Vector3(-12.3633f, -14.6075f, -0.0146f) ],
        [ShipStatus.MapType.Pb] = [
            new Vector3(3.5599f, -7.584f, -0.0076f),
            new Vector3(22.1169f, -25.0981f, -0.0251f),
            new Vector3(37.3687f, -21.9697f, -0.022f),
            new Vector3(40.6573f, -7.9562f, -0.008f) ],
        [ShipStatus.MapType.Hq] = [
            new Vector3(11.5355f, 10.3573f, 0.0104f),
            new Vector3(-3.063f, 3.8147f, 0.0038f),
            new Vector3(16.6542f, 25.3223f, 0.0253f),
            new Vector3(19.5728f, 17.4778f, 0.0175f) ],
        [ShipStatus.MapType.Fungle] = [
            new Vector3(-22.4063f, -1.6647f, -0.0017f),
            new Vector3(-11.0019f, 12.6502f, 0.0127f),
            new Vector3(24.3133f, 14.628f, 0.0146f),
            new Vector3(7.6678f, -9.9008f, -0.0099f)
            ],

        // Submerged compatibility soon
        [(ShipStatus.MapType)6] = []
    };

    private readonly Vector3[] _airshipPositions = [
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
        nodes = [];
        hackedPlayers = [];
    }

    private void OnDestroy()
    {
        nodes.Clear();
        hackedPlayers.Clear();
    }

    public bool AnyActiveNodes()
    {
        return nodes.Any(node => node.IsActive);
    }

    public static IEnumerator HackEffect()
    {
        var random = new Random();
        HudManager.Instance.TaskPanel.open = false;
        var originalPos = HudManager.Instance.ReportButton.transform.localPosition;
        var originalPos2 = HudManager.Instance.UseButton.transform.localPosition;
        var taskBar = HudManager.Instance.gameObject.GetComponentInChildren<ProgressTracker>();

        while (PlayerControl.LocalPlayer.Data.IsHacked())
        {
            HudManager.Instance.FullScreen.color = new Color32(0, 255, 0, 100);
            HudManager.Instance.FullScreen.gameObject.SetActive(!HudManager.Instance.FullScreen.gameObject.active);
            SoundManager.Instance.PlaySound(LaunchpadAssets.HackingSound.LoadAsset(), false, 0.6f);
            taskBar.curValue = random.NextSingle();
            if (random.Next(0, 2) == 1)
            {
                HudManager.Instance.TaskPanel.open = true;
                yield return new WaitForSeconds(0.1f);
                HudManager.Instance.TaskPanel.open = false;
            }

            if (random.Next(0, 2) == 1)
            {
                HudManager.Instance.ReportButton.transform.localPosition += new Vector3(-random.NextSingle() + 1, random.NextSingle() + 1);
                yield return new WaitForSeconds(0.2f);
                HudManager.Instance.ReportButton.transform.localPosition = originalPos;
            }

            if (random.Next(0, 2) == 1)
            {
                HudManager.Instance.UseButton.transform.localPosition += new Vector3(-random.NextSingle() + 1, random.NextSingle() + 1);
                yield return new WaitForSeconds(0.2f);
                HudManager.Instance.UseButton.transform.localPosition = originalPos2;
            }

            yield return new WaitForSeconds(0.6f);
        }

        if (HudManager.InstanceExists)
        {
            SoundManager.Instance.StopSound(LaunchpadAssets.HackingSound.LoadAsset());
            HudManager.Instance.FullScreen.gameObject.SetActive(false);
            HudManager.Instance.UseButton.transform.localPosition = originalPos2;
            HudManager.Instance.ReportButton.transform.localPosition = originalPos;
        }
    }


    [MethodRpc((uint)LaunchpadRPC.HackPlayer)]
    public static void RpcHackPlayer(PlayerControl player)
    {
        if (player.Data.Role is not HackerRole)
        {
            return;
        }
        
        Instance.hackedPlayers.Add(player.PlayerId);
        GradientManager.SetGradientEnabled(player, false);
        player.RawSetColor(15);

        if (player.AmOwner) Coroutines.Start(HackEffect());
    }

    [MethodRpc((uint)LaunchpadRPC.UnHackPlayer)]
    public static void RpcUnHackPlayer(PlayerControl player)
    {
        Instance.hackedPlayers.Remove(player.PlayerId);
        player.SetName(player.Data.PlayerName);
        GradientManager.SetGradientEnabled(player, true);
        player.SetColor((byte)player.Data.DefaultOutfit.ColorId);
        player.cosmetics.gameObject.SetActive(true);

        if (!AmongUsClient.Instance.AmHost || Instance.hackedPlayers.Count > 0)
        {
            return;
        }
        
        foreach (var node in Instance.nodes)
        {
            RpcToggleNode(PlayerControl.LocalPlayer, node.Id, false);
        }
    }

    [MethodRpc((uint)LaunchpadRPC.CreateNodes)]
    public static void RpcCreateNodes(ShipStatus shipStatus)
    {
        var nodesParent = new GameObject("Nodes");
        nodesParent.transform.SetParent(shipStatus.transform);

        var nodePositions = Instance._mapNodePositions[shipStatus.Type];
        if (shipStatus.TryCast<AirshipStatus>())
        {
            nodePositions = Instance._airshipPositions;
        }

        for (var i = 0; i < nodePositions.Length; i++)
        {
            var nodePos = nodePositions[i];
            Instance.CreateNode(shipStatus, i, nodesParent.transform, nodePos);
        }
    }

    [MethodRpc((uint)LaunchpadRPC.ToggleNode)]
    public static void RpcToggleNode(PlayerControl playerControl, int nodeId, bool value)
    {
        if (playerControl.Data.Role is not HackerRole || !AmongUsClient.Instance.AmHost)
        {
            return;
        }
        
        var node = Instance.nodes.Find(node => node.Id == nodeId);
        Debug.Log(node.gameObject.transform.position.ToString());
        node.IsActive = value;
        var hacker = GameData.Instance.AllPlayers.ToArray().Where(player => player.Role is HackerRole);
        foreach (var player in hacker)
        {
            player.Object.SetName(player.PlayerName);

            if (!value)
            {
                GradientManager.SetGradientEnabled(player.Object, true);
                player.Object.SetColor((byte)player.DefaultOutfit.ColorId);
            }
            else
            {
                GradientManager.SetGradientEnabled(player.Object, false);
                player.Object.RawSetColor(15);
            }

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
        nodes.Add(nodeComponent);
        return nodeComponent;
    }
}
