using LaunchpadReloaded.Buttons.Impostor;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Utilities;
using MiraAPI.Hud;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Impostor;

public class HitmanRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Hitman";
    public string RoleDescription => "Slow down time and kill the Crewmates.";
    public string RoleLongDescription => "Slow down time and kill the Crewmates.\nYou can kill multiple players at once.";
    public Color RoleColor => LaunchpadPalette.HitmanColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.DragButton,
        OptionsScreenshot = LaunchpadAssets.JanitorBanner,
        UseVanillaKillButton = false,
    };

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        if (player.HasModifier<RevealedModifier>()) return true;
        return PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer.Data.Role.IsImpostor;
    }

    public override void OnDeath(DeathReason reason)
    {
        Deinitialize(Player);
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        if (!targetPlayer.AmOwner)
        {
            return;
        }

        if (CustomButtonSingleton<DeadlockButton>.Instance.EffectActive)
        {
            HitmanUtilities.ClearMarks();
            CustomButtonSingleton<DeadlockButton>.Instance.OnEffectEnd();
        }
    }

    public bool InDeadlockMode = false;
    public GameObject? Overlay;
    public readonly Color OverlayColor = new(10f / 255f, 30f / 255f, 10f / 255f, 0.4f);

    public SpriteRenderer? _overlayRend { get; private set; }
    private IEnumerator? _transitionCoroutine = null;

    public void StartTransition(Color targetColor, Action? action = null)
    {
        if (_transitionCoroutine != null)
        {
            Coroutines.Stop(_transitionCoroutine);
        }

        _transitionCoroutine = Coroutines.Start(HitmanUtilities.TransitionColor(targetColor, _overlayRend!, 1f, action));
    }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        Overlay = GameObject.Find("DeadlockTint");

        if (Overlay == null)
        {
            Overlay = new("DeadlockTint");
            Overlay.gameObject.SetActive(false);
            Overlay.transform.SetParent(HudManager.Instance.transform, true);

            _overlayRend = Overlay.AddComponent<SpriteRenderer>();
            _overlayRend.sprite = LaunchpadAssets.DeadlockVignette.LoadAsset();
            _overlayRend.color = OverlayColor;

            Camera mainCamera = Camera.main;
            float screenHeight = mainCamera.orthographicSize * 2f;
            float screenWidth = screenHeight * mainCamera.aspect;
            float spriteWidth = _overlayRend.sprite.bounds.size.x;
            float spriteHeight = _overlayRend.sprite.bounds.size.y;

            Overlay.transform.localScale = new Vector3(screenWidth / spriteWidth, screenHeight / spriteHeight, 1f);
            var position = HudManager.Instance.transform.position;
            Overlay.transform.localPosition = new Vector3(position.x, position.y, 20f);
        }
    }

    public void FixedUpdate()
    {
        if (Overlay != null && Player.AmOwner)
        {
            Overlay.transform.position = HudManager.Instance.FullScreen.transform.position;
        }
    }
}