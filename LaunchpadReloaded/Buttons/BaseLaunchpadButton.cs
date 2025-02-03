using LaunchpadReloaded.Utilities;
using MiraAPI.Hud;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public abstract class BaseLaunchpadButton : CustomActionButton
{
#if ANDROID
    public override ButtonLocation Location => ButtonLocation.BottomRight;
#endif

    public abstract bool TimerAffectedByPlayer { get; }

    public abstract bool AffectedByHack { get; }

    public override bool CanUse()
    {
        var buttonTimer = !TimerAffectedByPlayer || PlayerControl.LocalPlayer.ButtonTimerEnabled();
        var hack = !AffectedByHack || !PlayerControl.LocalPlayer.Data.IsHacked();
        return base.CanUse() && buttonTimer && hack;
    }
}

public abstract class BaseLaunchpadButton<T> : CustomActionButton<T> where T : MonoBehaviour
{
#if ANDROID
    public override ButtonLocation Location => ButtonLocation.BottomRight;
#endif

    public abstract bool TimerAffectedByPlayer { get; }

    public abstract bool AffectedByHack { get; }

    public override bool CanUse()
    {
        var buttonTimer = !TimerAffectedByPlayer || PlayerControl.LocalPlayer.ButtonTimerEnabled();
        var hack = !AffectedByHack || !PlayerControl.LocalPlayer.Data.IsHacked();
        return base.CanUse() && buttonTimer && hack;
    }
}