﻿using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class ScannerComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public PlayerControl PlacedBy;
    public byte Id;
    public List<PlayerControl> PlayersInProximity = new List<PlayerControl>();
    public PlainShipRoom Room;

    public void Awake()
    {
        this.Room = Helpers.GetRoom(transform.position);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.gameObject.GetComponent<PlayerControl>();
        if (player == null) return;

        if (PlayerControl.LocalPlayer.Data.Role is TrackerRole)
        {
            Helpers.SendNotification($"<b>{Room.RoomId} Scanner:</b>{player.Data.Color.ToTextColor()} {player.Data.PlayerName}</color>", Color.white, 1.4f);
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
