using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using Reactor.Networking.Attributes;

namespace LaunchpadReloaded.Networking;

public static class DragRpc
{
    [MethodRpc((uint)LaunchpadRpc.StartDrag)]
    public static void RpcStartDragging(this PlayerControl playerControl, byte bodyId)
    {
        if (playerControl.Data.Role is not JanitorRole)
        {
            return;
        }
        
        DragManager.Instance.DraggingPlayers.Add(playerControl.PlayerId, bodyId);
        playerControl.MyPhysics.Speed = GameOptionsManager.Instance.currentNormalGameOptions.PlayerSpeedMod/2;
        if (playerControl.AmOwner)
        {
            CustomButtonSingleton<DragButton>.Instance.SetDrop();
        }
    }

    [MethodRpc((uint)LaunchpadRpc.StopDrag)]
    public static void RpcStopDragging(this PlayerControl playerControl)
    {
        DragManager.Instance.DraggingPlayers.Remove(playerControl.PlayerId);
        playerControl.MyPhysics.Speed = GameOptionsManager.Instance.currentNormalGameOptions.PlayerSpeedMod;
        if (playerControl.AmOwner)
        {
            CustomButtonSingleton<DragButton>.Instance.SetDrag();
        }
    }

}