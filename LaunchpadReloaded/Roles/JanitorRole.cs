using System;
using System.Linq;
using AmongUs.GameOptions;
using BepInEx.Configuration;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Attributes;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class JanitorRole : ImpostorRole, ICustomRole
{
    public string RoleName => "Janitor";
    public string RoleDescription => "Help the impostor by\ndragging and hiding bodies";
    public string RoleLongDescription => "Help the Impostor with your ability to move bodies around and bury them!";
    public Color RoleColor => Color.yellow;
    public RoleTeamTypes Team => RoleTeamTypes.Impostor;
    public CustomButton[] CustomButtons => [DragButton, BuryButton];
    
    public static CustomButton DragButton = new CustomButton(ToggleDrag, "DRAG", 0, "LaunchpadReloaded.Resources.drag.png");
    public static CustomButton BuryButton = new CustomButton(BuryBody, "BURY", 5, "LaunchpadReloaded.Resources.bury.png", 3);
    
    private DeadBody _target;
    private bool _dragging;

    public JanitorRole()
    {
        Debug.LogError("JANITOR INIT");
    }
    
    public JanitorRole(IntPtr ptr) : base(ptr)
    {
    }

    public void HudUpdate(HudManager hudManager)
    {
        if (_target != null)
        {
            DragButton.Button.SetEnabled();
            if (_dragging)
            {
                BuryButton.Button.SetDisabled();
                UpdateBodyPos(PlayerControl.LocalPlayer.PlayerId, _target.ParentId);
            }
            else
            {
                BuryButton.Button.SetEnabled();
            }
        }
        else
        {
            BuryButton.Button.SetDisabled();
            DragButton.Button.SetDisabled();
        }
    }
    
    public void PlayerControlFixedUpdate(PlayerControl playerControl)
    {
        _target = null;
        if (playerControl.IsKillTimerEnabled)
            _target = NearestDeadBody();
        
        foreach (var body in FindObjectsOfType<DeadBody>())
        {
            foreach (var bodyRenderer in body.bodyRenderers)
            {
                bodyRenderer.SetOutline(null);
            }
        }
        if (_target != null)
        {
            foreach (var bodyRenderer in _target.bodyRenderers)
            {
                bodyRenderer.SetOutline(TeamColor);
            }
        }
    }
    
    public static void UpdateBodyPos(byte draggerId, byte selected)
    {
        try
        {
            var bodyById = GetBodyById(selected);
            var dragger = GameData.Instance.GetPlayerById(draggerId).Object;
            bodyById.transform.position = !GameData.Instance.GetPlayerById(draggerId).Object.inVent ? Vector3.Lerp(bodyById.transform.position, dragger.transform.position, 4f * Time.deltaTime) : dragger.transform.position;
        }
        catch { }
    }
    
    public static DeadBody GetBodyById(byte id)
    {
        return FindObjectsOfType<DeadBody>().FirstOrDefault(body => body.ParentId == id);
    }
    
    private static void ToggleDrag()
    {
        if (PlayerControl.LocalPlayer.Data.Role is JanitorRole role)
        {
            role._dragging = !role._dragging;
        }
    }

    private static void BuryBody()
    {
        if (PlayerControl.LocalPlayer.Data.Role is JanitorRole role)
        {
            role._target.Reported = true;
            foreach (var bodyRenderer in role._target.bodyRenderers)
            {
                bodyRenderer.enabled = false;
            }
        }
    }
    
    public static DeadBody NearestDeadBody()
    {
        return Physics2D
            .OverlapCircleAll(PlayerControl.LocalPlayer.transform.position, 1f, ~LayerMask.GetMask(new[] {"Ship"}))
            .Where(collider2D => collider2D.CompareTag("DeadBody"))
            .Select(collider2D => collider2D.GetComponent<DeadBody>()).FirstOrDefault(component => component && !component.Reported);
    }
    
}