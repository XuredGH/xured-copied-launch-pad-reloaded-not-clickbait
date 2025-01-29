using LaunchpadReloaded.Options.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Modifiers;

[RegisterModifier]
public class DragBodyModifier : BaseModifier
{
    public override string ModifierName => "Drag Body";

    public override bool HideOnUi => true;

    private byte BodyId { get; }
    private DeadBody DeadBody { get; }

    public DragBodyModifier(byte bodyId)
    {
        BodyId = bodyId;
        DeadBody = Helpers.GetBodyById(BodyId);
    }

    public override void OnActivate()
    {
        if (Player != null)
        {
            Player.MyPhysics.Speed = OptionGroupSingleton<JanitorOptions>.Instance.DragSpeed;
        }
    }

    public override void OnDeactivate()
    {
        if (Player == null)
        {
            return;
        }

        Player.MyPhysics.Speed = 2.5f;
    }

    public override void Update()
    {
        if (BodyId == 255)
        {
            return;
        }

        if (!DeadBody)
        {
            return;
        }

        DeadBody.transform.position = Vector3.Lerp(DeadBody.transform.position, Player.transform.position, 5f * Time.deltaTime);
    }
}