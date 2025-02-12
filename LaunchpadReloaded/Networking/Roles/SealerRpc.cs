using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using Reactor.Networking.Attributes;
using System.Linq;
using UnityEngine;

namespace LaunchpadReloaded.Networking.Roles;
public static class SealerRpc
{
    [MethodRpc((uint)LaunchpadRpc.SealVent)]
    public static void RpcSealVent(this PlayerControl playerControl, int ventId)
    {
        if (playerControl.Data.Role is not LocksmithRole sealer)
        {
            playerControl.KickForCheating();
            return;
        }
        var vent = ShipStatus.Instance.AllVents.FirstOrDefault(v => v.Id == ventId);

        if (vent is null)
        {
            return;
        }

        if (OptionGroupSingleton<LocksmithOptions>.Instance.SealReveal && vent.gameObject.TryGetComponent<VentBodyComponent>(out var body))
        {
            body.ExposeBody();
        }

        var seal = vent.gameObject.AddComponent<SealedVentComponent>();
        seal.Sealer = playerControl;

        sealer.SealedVents.Add(seal);

        GameObject ventTape = new GameObject("VentTape");
        ventTape.transform.SetParent(vent.transform);
        ventTape.transform.localPosition = new Vector3(0, -0.05f, -0.05f);
        ventTape.transform.localScale = new Vector3(0.55f, 0.35f, 0.4f);
        ventTape.layer = vent.gameObject.layer;

        SpriteRenderer rend = ventTape.AddComponent<SpriteRenderer>();
        rend.sprite = LaunchpadAssets.VentTape.LoadAsset();
        rend.color = new UnityEngine.Color(1, 1, 1, 0.5f);

        //TODO; IMPLEMENT DIFFERENT TEXTURES FOR MAPS.
    }
}