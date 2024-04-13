using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;

namespace LaunchpadReloaded.Networking;
public static class GenericRpc
{
    [MethodRpc((uint)LaunchpadRpc.Revive)]
    public static void RpcRevive(this PlayerControl playerControl, byte bodyId)
    {
        if (playerControl.Data.Role is not MedicRole)
        {
            playerControl.KickForCheating();
            return;
        }

        var body = Helpers.GetBodyById(bodyId);
        if (body)
        {
            body.Revive();
        }
        else
        {
            Logger<LaunchpadReloadedPlugin>.Warning($"Body for id {bodyId} not found");
        }
    }
}