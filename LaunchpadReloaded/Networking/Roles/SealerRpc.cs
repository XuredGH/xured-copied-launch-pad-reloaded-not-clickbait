using LaunchpadReloaded.Components;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using Reactor.Networking.Attributes;
using System.Linq;

namespace LaunchpadReloaded.Networking.Roles;
public static class SealerRpc
{
    [MethodRpc((uint)LaunchpadRpc.SealVent)]
    public static void RpcSealVent(this PlayerControl playerControl, int ventId)
    {
        if (playerControl.Data.Role is not SealerRole sealer)
        {
            playerControl.KickForCheating();
            return;
        }
        var vent = ShipStatus.Instance.AllVents.FirstOrDefault(v => v.Id == ventId);

        if (vent is null)
        {
            return;
        }

        if (OptionGroupSingleton<SealerOptions>.Instance.SealReveal && vent.gameObject.TryGetComponent<VentBodyComponent>(out var body))
        {
            body.ExposeBody();
        }

        var seal = vent.gameObject.AddComponent<SealedVentComponent>();
        seal.Sealer = playerControl;

        sealer.SealedVents.Add(seal);
    }
}