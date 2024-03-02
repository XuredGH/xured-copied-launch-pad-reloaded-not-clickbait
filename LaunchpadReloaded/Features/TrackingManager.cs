using LaunchpadReloaded.Roles;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.Features;
public class TrackingManager
{
    public static PlayerControl TrackedPlayer = null;
    public static Vector3 MapPosition;
    public static float Timer = 0f;
    public static AudioClip PingSound = LaunchpadReloadedPlugin.Bundle.LoadAsset<AudioClip>("Ping.mp3");
    public static bool TrackerDisconnected = false;

    public static void TrackingUpdate()
    {
        if (Timer <= 0f)
        {
            Vector3 vector = TrackedPlayer.transform.position;
            vector /= ShipStatus.Instance.MapScale;
            vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
            vector.z = -1f;
            MapPosition = vector;
            SoundManager.Instance.PlaySound(PingSound, false, 0.8f, null);

            // Stop pinging when player dies
            if (TrackedPlayer.Data.IsDead || TrackedPlayer.Data.Disconnected) TrackerDisconnected = true;

            Timer = 7f;
        }
        else
        {
            Timer -= Time.deltaTime;
        }
        return;
    }
}