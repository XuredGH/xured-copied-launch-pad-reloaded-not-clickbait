using LaunchpadReloaded.Components;
using LaunchpadReloaded.Utilities;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Usables;

namespace LaunchpadReloaded;

public static class LaunchpadEventListeners
{
    public static void Initialize()
    {
        MiraEventManager.RegisterEventHandler<StartMeetingEvent>(StartMeetingEvent);
        MiraEventManager.RegisterEventHandler<PlayerCanUseEvent>(CanUseEvent, 10);
    }

    // prevent meetings during hack
    public static void StartMeetingEvent(StartMeetingEvent meetingEvent)
    {
        if (HackerUtilities.AnyPlayerHacked())
        {
            meetingEvent.Cancel();
        }
    }

    // prevent tasks during hack
    public static void CanUseEvent(PlayerCanUseEvent @event)
    {
        if (@event.IsVent)
        {
            var vent = @event.Usable.Cast<Vent>();
            if (vent.gameObject.GetComponent<SealedVentComponent>())
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