using System;
using System.Collections.Generic;
using LaunchpadReloaded.Components;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Features.Managers;
[RegisterInIl2Cpp]
public class ScannerManager(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static ScannerManager Instance;
    public List<ScannerComponent> scanners;

    private void Awake()
    {
        Instance = this;
        scanners = new List<ScannerComponent>();
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
        realCollision.radius = 0.2f;
        realCollision.offset = new Vector2(0, -0.2f);

        var component = scanner.AddComponent<ScannerComponent>();
        component.PlacedBy = playerControl;
        component.Id = (byte)(scanners.Count + 1);

        scanner.SetActive(true);

        Logger<LaunchpadReloadedPlugin>.Info($"Scanner {component.Id} placed by {playerControl.Data.PlayerName}");
        return component;
    }
}
