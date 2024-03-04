using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using Reactor.Networking.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ShipStatus;

namespace LaunchpadReloaded.Features;
public static class ScannerManager
{
    public static List<ScannerComponent> Scanners = new();

    [MethodRpc((uint)LaunchpadRPC.CreateScanner)]
    public static void RpcCreateScanner(PlayerControl playerControl)
    {
        Scanners.Add(CreateScanner(playerControl));
    }

    public static ScannerComponent CreateScanner(PlayerControl playerControl)
    {
        var scanner = new GameObject("Scanner");
        scanner.transform.localPosition = PlayerControl.LocalPlayer.transform.localPosition;

        var sprite = scanner.AddComponent<SpriteRenderer>();
        sprite.sprite = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Scanner.png");
        scanner.layer = LayerMask.NameToLayer("ShortObjects");
        sprite.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        sprite.material = ShipStatus.Instance.AllConsoles[0].gameObject.GetComponent<SpriteRenderer>().material;

        var collider = scanner.AddComponent<CircleCollider2D>();
        collider.radius = 3.5f;
        collider.isTrigger = true;

        var component = scanner.AddComponent<ScannerComponent>();
        component.PlacedBy = playerControl;
        component.Id = (byte)(Scanners.Count + 1);

        scanner.SetActive(true);

        if (playerControl.Data.Role is TrackerRole trackerRole) trackerRole.PlacedScanners.Add(component);

        Debug.Log($"Scanner {component.Id} placed by {playerControl.Data.PlayerName}");
        return component;
    }
}
