using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Features;
[RegisterInIl2Cpp]
public class TrackingManager(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static TrackingManager Instance;
    public PlayerControl TrackedPlayer = null;
    public Vector3 MapPosition;
    public float Timer = 0f;
    public bool TrackerDisconnected = false;

    private void Awake()
    {
        Instance = this;
    }

    public void TrackingUpdate()
    {
        if (Timer <= 0f)
        {
            Vector3 vector = TrackedPlayer.transform.position;
            vector /= ShipStatus.Instance.MapScale;
            vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
            vector.z = -1f;
            MapPosition = vector;
            SoundManager.Instance.PlaySound(LaunchpadAssets.PingSound.LoadAsset(), false, 0.8f, null);

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