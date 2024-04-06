using System;
using System.Collections.Generic;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class ScannerComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public PlayerControl placedBy;
    public byte id;
    public List<PlayerControl> playersInProximity = [];
    public PlainShipRoom room;

    public void Awake()
    {
        room = Helpers.GetRoom(transform.position);
        if (room)
        {
            return;
        }
        
        room = gameObject.AddComponent<PlainShipRoom>();
        switch (ShipStatus.Instance.Type)
        {
            case ShipStatus.MapType.Hq:
            case ShipStatus.MapType.Ship:
                room.RoomId = SystemTypes.Hallway;
                break;
            case ShipStatus.MapType.Pb:
            case ShipStatus.MapType.Fungle:
            default:
                room.RoomId = SystemTypes.Outside;
                break;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (HackingManager.Instance && HackingManager.Instance.AnyPlayerHacked())
        {
            return;
        }

        var player = collider.gameObject.GetComponent<PlayerControl>();
        if (player == null)
        {
            return;
        }

        if (PlayerControl.LocalPlayer.Data.Role is TrackerRole)
        {
            Helpers.SendNotification($"<b>{room.RoomId} Scanner:</b>{player.Data.Color.ToTextColor()} {player.Data.PlayerName}</color>", Color.white, 1.4f);
            SoundManager.Instance.PlaySoundImmediate(LaunchpadAssets.BeepSound.LoadAsset(), false, 0.3f);
            return;
        }

        if (player.AmOwner)
        {
            SoundManager.Instance.PlaySoundImmediate(LaunchpadAssets.BeepSound.LoadAsset(), false, 0.5f);
            Helpers.SendNotification($"<b>You have triggered the scanner.</b>\n{LaunchpadPalette.TrackerColor.ToTextColor()}The Tracker will be notified.</color>", Color.white, 1.4f, 2.4f);
        }
    }
}
