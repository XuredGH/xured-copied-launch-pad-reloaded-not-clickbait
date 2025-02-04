using LaunchpadReloaded.Utilities;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla;

namespace LaunchpadReloaded;

public static class LaunchpadEventListeners
{
    public static void Initialize()
    {
        MiraEventManager.RegisterEventHandler<StartMeetingEvent>(StartMeetingEvent);
        MiraEventManager.RegisterEventHandler<UseButtonClickEvent>(UseButtonEvent);
    }

    public static void StartMeetingEvent(StartMeetingEvent meetingEvent)
    {
        if (HackerUtilities.AnyPlayerHacked())
        {
            meetingEvent.Cancel();
        }

    }

    public static void UseButtonEvent(UseButtonClickEvent useButtonEvent)
    {
        if (HackerUtilities.AnyPlayerHacked() && !useButtonEvent.Button.currentTarget.UsableWhenHacked())
        {
            useButtonEvent.Cancel();
        }
    }
}