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
        Icon = LaunchpadAssets.DeadlockButton,
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

    private SpriteRenderer? _overlayRend;
    private IEnumerator? _transitionCoroutine = null;

    public void StartTransition(Color targetColor)
    {
        if (_transitionCoroutine != null)
        {
            Coroutines.Stop(_transitionCoroutine);
        }

        _transitionCoroutine = Coroutines.Start(HitmanUtilities.TransitionColor(targetColor, _overlayRend!));
    }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        Overlay = GameObject.Find("OverlayTint");

        if (Overlay == null)
        {
            Overlay = Instantiate(HudManager.Instance.FullScreen).gameObject;
            Overlay.gameObject.SetActive(false);
            Overlay.name = "OverlayTint";
            Overlay.transform.SetParent(HudManager.Instance.transform, true);

            var position = HudManager.Instance.transform.position;
            Overlay.transform.localPosition = new Vector3(position.x, position.y, 20f);
        }

        _overlayRend = Overlay.GetComponent<SpriteRenderer>();
    }

    public void FixedUpdate()
    {
        if (Overlay != null && Player.AmOwner)
        {
            Overlay.transform.position = HudManager.Instance.FullScreen.transform.position;
        }
    }
}