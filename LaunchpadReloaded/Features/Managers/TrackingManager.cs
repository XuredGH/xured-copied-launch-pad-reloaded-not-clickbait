using System;
using LaunchpadReloaded.Roles;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Features.Managers;
[RegisterInIl2Cpp]
public class TrackingManager(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static TrackingManager Instance;
    public PlayerControl TrackedPlayer;
    public Vector3 MapPosition;
    public float Timer;
    public bool TrackerDisconnected;

    private void Awake()
    {
        Instance = this;
        TrackedPlayer = null;
    }

    public void TrackingUpdate()
    {
        if (Timer <= 0f)
        {
            var vector = TrackedPlayer.transform.position;
            vector /= ShipStatus.Instance.MapScale;
            vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
            vector.z = -1f;
            MapPosition = vector;
            SoundManager.Instance.PlaySound(LaunchpadAssets.PingSound.LoadAsset(), false, 0.8f);

            // Stop pinging when player dies
            if (TrackedPlayer.Data.IsDead || TrackedPlayer.Data.Disconnected)
            {
                TrackerDisconnected = true;
            }

            Timer = TrackerRole.PingTimer.Value;
        }
        else
        {
            Timer -= Time.deltaTime;
        }
    }
}