using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LaunchpadReloaded.Features;
[RegisterInIl2Cpp]
public class ScannerManager(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static ScannerManager Instance;
    public List<ScannerComponent> Scanners = new();

    private void Awake()
    {
        Instance = this;
    }

    [MethodRpc((uint)LaunchpadRPC.CreateScanner)]
    public static void RpcCreateScanner(PlayerControl playerControl, float x, float y)
    {
        ScannerComponent newScanner = Instance.CreateScanner(playerControl, new Vector3(x, y, 0.0057f));
        Instance.Scanners.Add(newScanner);
    }

    public ScannerComponent CreateScanner(PlayerControl playerControl, Vector3 pos)
    {
        var scanner = new GameObject("Scanner");
        scanner.transform.position = pos;
        scanner.transform.SetParent(ShipStatus.Instance.transform);

        var sprite = scanner.AddComponent<SpriteRenderer>();
        sprite.sprite = LaunchpadAssets.ScannerSprite.LoadAsset();
        scanner.layer = LayerMask.NameToLayer("Ship");
        sprite.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        var collider = scanner.AddComponent<CircleCollider2D>();
        collider.radius = 2f;
        collider.isTrigger = true;

        var realCollision = scanner.AddComponent<CircleCollider2D>();
        realCollision.radius = 0.6f;
        realCollision.offset = new Vector2(0, -0.2f);

        var component = scanner.AddComponent<ScannerComponent>();
        component.PlacedBy = playerControl;
        component.Id = (byte)(Scanners.Count + 1);

        scanner.SetActive(true);

        Debug.Log($"Scanner {component.Id} placed by {playerControl.Data.PlayerName}");
        return component;
    }
}
