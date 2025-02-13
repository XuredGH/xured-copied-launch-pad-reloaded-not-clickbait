﻿using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Utilities;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Map;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Usables;
using MiraAPI.Utilities;

namespace LaunchpadReloaded;

public static class LaunchpadEventListeners
{
    public static void Initialize()
    {
        MiraEventManager.RegisterEventHandler<StartMeetingEvent>(StartMeetingEvent);
        //  MiraEventManager.RegisterEventHandler<EjectionEvent>(EjectEvent); BROKEN
        MiraEventManager.RegisterEventHandler<PlayerCanUseEvent>(CanUseEvent, 10);

        MiraEventManager.RegisterEventHandler<PlayerOpenSabotageEvent>(@event =>
        {
            if (PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>())
            {
                @event.Cancel();
            }
        });
    }

    public static void EjectEvent(EjectionEvent @event)
    {
        var tagManager = PlayerControl.LocalPlayer.GetTagManager();
        if (tagManager != null)
        {
            tagManager.MeetingEnd();
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