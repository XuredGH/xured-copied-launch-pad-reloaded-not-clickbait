using LaunchpadReloaded.Utilities;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla;
using MiraAPI.Events.Vanilla.Meeting;

namespace LaunchpadReloaded;

public static class LaunchpadEventListeners
{
    public static void Initialize()
    {
        MiraEventManager.RegisterEventHandler<StartMeetingEvent>(StartMeetingEvent);
        MiraEventManager.RegisterEventHandler<UseButtonClickEvent>(UseButtonEvent);
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
    public static void UseButtonEvent(UseButtonClickEvent useButtonEvent)
    {
        if (PlayerControl.LocalPlayer.Data.IsHacked() && !useButtonEvent.Button.currentTarget.UsableWhenHacked())
        {
            useButtonEvent.Cancel();
        }
    }
}