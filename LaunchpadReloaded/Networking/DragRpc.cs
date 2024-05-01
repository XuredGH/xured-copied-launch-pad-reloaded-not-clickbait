using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Networking.Attributes;

namespace LaunchpadReloaded.Networking;

public static class DragRpc
{
    [MethodRpc((uint)LaunchpadRpc.StartDrag)]
    public static void RpcStartDragging(this PlayerControl playerControl, byte bodyId)
    {
        var role = playerControl.Data.Role;
        if (role is not JanitorRole && role is not MedicRole)
        {
            playerControl.KickForCheating();
            return;
        }

        playerControl.GetLpPlayer().dragId = bodyId;
        playerControl.MyPhysics.Speed = 1.5f;
        if (playerControl.AmOwner)
        {
            CustomButtonSingleton<DragButton>.Instance.SetDrop();
        }
    }

    [MethodRpc((uint)LaunchpadRpc.StopDrag)]
    public static void RpcStopDragging(this PlayerControl playerControl)
    {
        playerControl.GetLpPlayer().dragId = 255;
        playerControl.MyPhysics.Speed = 2.5f;
        if (playerControl.AmOwner)
        {
            CustomButtonSingleton<DragButton>.Instance.SetDrag();
        }
    }

}