using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Utilities;

public static class Helpers
{
    public static bool ShouldCancelClick()
    {
        return DragManager.IsDragging(PlayerControl.LocalPlayer.PlayerId);
    }
}