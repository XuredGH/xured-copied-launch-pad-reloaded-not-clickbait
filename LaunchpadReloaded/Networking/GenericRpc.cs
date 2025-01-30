using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Helpers = MiraAPI.Utilities.Helpers;

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
        if (body != null)
        {
            body.Revive();
        }
        else
        {
            Logger<LaunchpadReloadedPlugin>.Warning($"Body for id {bodyId} not found");
        }
    }

    [MethodRpc((uint)LaunchpadRpc.Poison)]
    public static void RpcPoison(this PlayerControl playerControl, PlayerControl victim, int time)
    {
        if (playerControl.Data.Role is not SurgeonRole)
        {
            playerControl.KickForCheating();
            return;
        }

        var poison = new PoisonModifier(playerControl, time);
        victim.GetModifierComponent()!.AddModifier(poison);
    }
}