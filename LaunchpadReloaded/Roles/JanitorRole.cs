using System;
using LaunchpadReloaded.API.Roles;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class JanitorRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Janitor";
    public string RoleDescription => "Help the impostor by\ndragging and hiding bodies";
    public string RoleLongDescription => "Help the Impostor with your ability to move bodies around and bury them!";
    public Color RoleColor => Color.yellow;
    public RoleTeamTypes Team => RoleTeamTypes.Impostor;
    
    private DeadBody _target;
    private bool _dragging;

    /*public void HudUpdate(HudManager hudManager)
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
    */
}