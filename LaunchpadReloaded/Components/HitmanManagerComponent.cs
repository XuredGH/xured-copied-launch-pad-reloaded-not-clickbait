using UnityEngine;
using System.Collections;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles.Impostor;
using MS.Internal.Xml.XPath;
using MiraAPI.Networking;

namespace LaunchpadReloaded.Components;

public class HitmanManagerComponent : MonoBehaviour
{
    public float transitionSpeed = 1f;
    private SpriteRenderer overlayRenderer;
    private Color deadEyeColor = new(2f / 255f, 48f / 255f, 32f / 255f, 0.7f); // Greenish tint for Dead Eye effect
    private Color transparentColor = new(0f, 0f, 0f, 0f);
    private Coroutine transitionCoroutine;
    private GameObject overlay;
    public bool isDeadEyeActive { get; private set; } = false;
    private AudioClip DeadEyeIn;
    private AudioClip DeadEyeOut;
    private AudioClip DeadEyeLoop;
    private AudioClip LoseHonorSFX;
    private AudioClip HitSFX;
    private AudioClip ClockLeftSFX;
    private AudioClip ClockRightSFX;
    private Sprite LoseHonorSprite;
    private Sprite TargetSprite;
    public PlayerControl Player;
    private List<PlayerControl> KillQueue = [];
    private float deadEyeTimer = 0f;
    public float deadEyeLimit;
    private float markCooldown = 1f;
    private float lastMarkTime = 0f;
    private float lastDeadEyeTime = -30f;
    public float deadEyeCooldown = 30f;

    public float ogShakeAmount;
    public float ogShakePeriod;
    public bool ogShouldShake;
    public float ogSpeed;
    private FollowerCamera followerCamera;
    private Coroutine clockTickCoroutine;

    public void Initialize()
    {
        overlay = GameObject.Find("flash222");

        if (overlay == null)
        {
            overlay = Instantiate(HudManager.Instance.FullScreen).gameObject;
            overlay.gameObject.SetActive(false);
            overlay.name = "flash222";
            overlay.transform.SetParent(HudManager.Instance.transform, true);
            var position = HudManager.Instance.transform.position;
            overlay.transform.localPosition = new Vector3(position.x, position.y, 20f);
        }

        DeadEyeIn = LaunchpadAssets.DeadlockFadeIn.LoadAsset();
        DeadEyeOut = LaunchpadAssets.DeadlockFadeOut.LoadAsset();
        DeadEyeLoop = LaunchpadAssets.DeadlockAmbience.LoadAsset();
        LoseHonorSFX = LaunchpadAssets.DeadlockKillConfirmal.LoadAsset();
        HitSFX = LaunchpadAssets.DeadlockMark.LoadAsset();
        ClockLeftSFX = LaunchpadAssets.DeadlockClockLeft.LoadAsset();
        ClockRightSFX = LaunchpadAssets.DeadlockClockRight.LoadAsset();
        LoseHonorSprite = LaunchpadAssets.DeadlockHonor.LoadAsset();
        TargetSprite = LaunchpadAssets.DeadlockTarget.LoadAsset();

        overlayRenderer = overlay.GetComponent<SpriteRenderer>();
        if (overlayRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on OverlayTint.");
        }

        followerCamera = Camera.main.GetComponent<FollowerCamera>();
    }

    void Update()
    {
        if (PlayerControl.LocalPlayer == null || !HudManager.InstanceExists)
        {
            return;
        }

        if (isDeadEyeActive)
        {
            deadEyeTimer += Time.unscaledDeltaTime;

            if (deadEyeTimer >= deadEyeLimit)
            {
                StopDeadEye();
            }
        }

        if (Input.GetMouseButtonDown(0) && isDeadEyeActive && Time.unscaledTime - lastMarkTime >= markCooldown)
        {
            lastMarkTime = Time.unscaledTime;
            MarkPlayer(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (overlay != null)
        {
            overlay.transform.position = HudManager.Instance.FullScreen.transform.position;
        }
    }

    private IEnumerator ClockTickRoutine()
    {
        while (isDeadEyeActive)
        {
            float tickInterval = Mathf.Lerp(1.5f, 0.2f, deadEyeTimer / deadEyeLimit);
            SoundManager.Instance.StopSound(ClockLeftSFX);
            SoundManager.Instance.PlaySound(ClockLeftSFX, false, 0.5f);
            yield return new WaitForSeconds(tickInterval);
            SoundManager.Instance.StopSound(ClockRightSFX);
            SoundManager.Instance.PlaySound(ClockRightSFX, false, 0.5f);
            yield return new WaitForSeconds(tickInterval);
        }
    }

    public void StartDeadEye()
    {
        lastDeadEyeTime = Time.unscaledTime;
        lastMarkTime = Time.unscaledTime;
        deadEyeTimer = 0f;
        isDeadEyeActive = true;
        ogShakeAmount = followerCamera.shakeAmount;
        ogShakePeriod = followerCamera.shakePeriod;
        ogShouldShake = DataManager.Settings.Gameplay.ScreenShake;
        ogSpeed = PlayerControl.LocalPlayer.MyPhysics.Speed;
        deadEyeTimer = 0f;
        followerCamera.shakeAmount = 0.15f;
        followerCamera.shakePeriod = 1.2f;
        DataManager.Settings.Gameplay.ScreenShake = true;
        PlayerControl.LocalPlayer.MyPhysics.Speed = ogSpeed * 0.75f;
        StartTransition(deadEyeColor);
        HudManager.Instance?.AlertFlash?.SetOverlay(true);
        SoundManager.Instance.StopSound(DeadEyeIn);
        SoundManager.Instance.PlaySound(DeadEyeIn, false);
        SoundManager.Instance.PlaySound(DeadEyeLoop, true);
        clockTickCoroutine = StartCoroutine(ClockTickRoutine().WrapToIl2Cpp());
    }

    public void StopDeadEye()
    {
        lastDeadEyeTime = Time.unscaledTime;
        lastMarkTime = Time.unscaledTime;
        deadEyeTimer = 0f;
        isDeadEyeActive = false;
        StartTransition(transparentColor);
        HudManager.Instance?.AlertFlash?.SetOverlay(false);
        SoundManager.Instance.StopSound(DeadEyeOut);
        SoundManager.Instance.PlaySound(DeadEyeOut, false);
        SoundManager.Instance.StopSound(DeadEyeLoop);
        if (clockTickCoroutine != null) StopCoroutine(clockTickCoroutine);
        //Time.timeScale = 1.0f;
        followerCamera.shakeAmount = ogShakeAmount;
        followerCamera.shakePeriod = ogShakePeriod;
        DataManager.Settings.Gameplay.ScreenShake = ogShouldShake;
        PlayerControl.LocalPlayer.MyPhysics.Speed = ogSpeed;

        if (KillQueue.Count > 0)
        {
            StartCoroutine(ProcessKillQueue().WrapToIl2Cpp());
        }
    }

    private void MarkPlayer(Vector3 position)
    {
        if (KillQueue.Count >= 2) return;

        Vector3 mousePosition = position;
        mousePosition.z = 0f;

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);

        foreach (Collider2D hitCollider in hitColliders)
        {
            PlayerControl playerControl = hitCollider.GetComponent<PlayerControl>();
            if (playerControl != null)
            {
                if (KillQueue.Contains(playerControl) || playerControl == PlayerControl.LocalPlayer)
                {
                    continue;
                }
                GameObject mark = new("Mark");
                mark.transform.SetParent(playerControl.transform);
                mark.transform.localPosition = new Vector3(0f, 0f, -1f);
                mark.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
                var rend = mark.gameObject.AddComponent<SpriteRenderer>();
                rend.sprite = TargetSprite;
                KillQueue.Add(playerControl);
                SoundManager.Instance.StopSound(HitSFX);
                SoundManager.Instance.PlaySound(HitSFX, false);
                break;
            }
        }
    }

    private IEnumerator ProcessKillQueue()
    {
        var origCount = KillQueue.Count;
        while (KillQueue.Count > 0)
        {
            PlayerControl player = KillQueue[0];
            KillQueue.RemoveAt(0);

            if (player != null)
            {
                Player.RpcCustomMurder(player, resetKillTimer: false, createDeadBody: true, teleportMurderer: false, showKillAnim: true);
                var mark = player.transform.Find("Mark");
                if (mark != null)
                {
                    Destroy(mark.gameObject);
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
        if (origCount > 0)
        {
            Helpers.AddMessage($"  <size=130%>- (x{origCount})</size>", LoseHonorSprite, LoseHonorSFX, Color.red, out _);
        }
    }

    private void StartTransition(Color targetColor)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(TransitionColor(targetColor).WrapToIl2Cpp());
    }

    private IEnumerator TransitionColor(Color targetColor)
    {
        Color startColor = overlayRenderer.color;
        float elapsedTime = 0f;

        while (elapsedTime < transitionSpeed)
        {
            overlayRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / transitionSpeed);
            elapsedTime += Time.deltaTime * 4f;
            yield return null;
        }

        overlayRenderer.color = targetColor;
    }
}
