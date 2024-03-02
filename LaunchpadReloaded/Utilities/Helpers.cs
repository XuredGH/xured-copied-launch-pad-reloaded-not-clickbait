using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Utilities;

public static class Helpers
{
    public static bool ShouldCancelClick()
    {
        return DragManager.DraggingPlayers.ContainsKey(PlayerControl.LocalPlayer.PlayerId);
    }
}