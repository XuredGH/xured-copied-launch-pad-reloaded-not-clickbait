using LaunchpadReloaded.Components;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Utilities;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Map;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Usables;
using MiraAPI.Roles;
using MiraAPI.Utilities;

namespace LaunchpadReloaded;

public static class LaunchpadEventListeners
{
    public static void Initialize()
    {
        MiraEventManager.RegisterEventHandler<StartMeetingEvent>(StartMeetingEvent);
        MiraEventManager.RegisterEventHandler<EjectionEvent>(EjectEvent);
        MiraEventManager.RegisterEventHandler<PlayerCanUseEvent>(CanUseEvent, 10);
        MiraEventManager.RegisterEventHandler<SetRoleEvent>(SetRoleEvent);

        MiraEventManager.RegisterEventHandler<PlayerOpenSabotageEvent>(@event =>
        {
            if (PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>())
            {
                @event.Cancel();
            }
        });
    }

    public static void SetRoleEvent(SetRoleEvent @event)
    {
        var tagManager = @event.Player.GetTagManager();

        if (tagManager == null)
        {
            return;
        }

        var existingRoleTag = tagManager.GetTagByName("Role");
        if (existingRoleTag.HasValue)
        {
            tagManager.RemoveTag(existingRoleTag.Value);
        }

        var role = @event.Player.Data.Role;
        var color = role is ICustomRole custom ? custom.RoleColor : role.TeamColor;
        var name = role.NiceName;

        if (role.IsDead && name == "STRMISS")
        {
            name = "Ghost";
        }

        var roleTag = new PlayerTag()
        {
            Name = "Role",
            Text = name,
            Color = color,
            IsLocallyVisible = (player) =>
            {
                var plrRole = player.Data.Role;

                if (player.HasModifier<RevealedModifier>())
                {
                    return true;
                }

                if (plrRole is ICustomRole customRole && (player.AmOwner || customRole.CanLocalPlayerSeeRole(player)))
                {
                    return true;
                }

                if (player.AmOwner || PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer.Data.Role.IsImpostor && plrRole.IsImpostor)
                {
                    return true;
                }

                return false;
            },
        };

        tagManager.AddTag(roleTag);
    }
    public static void EjectEvent(EjectionEvent @event)
    {
        foreach (var plr in PlayerControl.AllPlayerControls)
        {
            var tagManager = plr.GetTagManager();
            if (tagManager != null)
            {
                tagManager.MeetingEnd();
            }
        }
    }

    // prevent meetings during hack
    public static void StartMeetingEvent(StartMeetingEvent meetingEvent)
    {
        if (HackerUtilities.AnyPlayerHacked() || meetingEvent.Reporter.HasModifier<DragBodyModifier>())
        {
            meetingEvent.Cancel();
            return;
        }

        if (PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>())
        {
            PlayerControl.LocalPlayer.RpcRemoveModifier<DragBodyModifier>();
        }
    }

    // prevent tasks during hack
    public static void CanUseEvent(PlayerCanUseEvent @event)
    {
        if (PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>())
        {
            @event.Cancel();
            return;
        }

        if (@event.IsVent)
        {
            var vent = @event.Usable.Cast<Vent>();
            if (vent.IsSealed())
            {
                @event.Cancel();
            }
        }

        if (PlayerControl.LocalPlayer && PlayerControl.LocalPlayer.Data.IsHacked() && @event.IsPrimaryConsole)
        {
            @event.Cancel();
        }

        if (HackerUtilities.AnyPlayerHacked() &&
            @event.Usable.TryCast<SystemConsole>() || @event.Usable.TryCast<MapConsole>())
        {
            @event.Cancel();
        }
    }
}