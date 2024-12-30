using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using UnityEngine;

namespace LaunchpadReloaded.Modifiers;

public class FootstepsModifier : BaseModifier
{
    public override string ModifierName => "Footsteps";

    public override bool HideOnUi => true;

    private Vector3 _lastPos;

    public override void OnActivate()
    {
        _lastPos = Player!.transform.position;
    }

    public override void Update()
    {
        if (Vector3.Distance(_lastPos, Player!.transform.position) > 1)
        {
            var angle = Mathf.Atan2(Player.MyPhysics.Velocity.y, Player.MyPhysics.Velocity.x) * Mathf.Rad2Deg;

            var footstep = new GameObject("Footstep")
            {
                transform =
                {
                    parent = ShipStatus.Instance.transform,
                    position = new Vector3(Player.transform.position.x, Player.transform.position.y, 2.5708f),
                    rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward)
                }
            };

            var sprite = footstep.AddComponent<SpriteRenderer>();
            sprite.sprite = LaunchpadAssets.Footstep.LoadAsset();
            sprite.material = LaunchpadAssets.GradientMaterial.LoadAsset();
            footstep.layer = LayerMask.NameToLayer("Players");

            sprite.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            Player.SetPlayerMaterialColors(sprite);

            _lastPos = Player.transform.position;
            Object.Destroy(footstep, OptionGroupSingleton<DetectiveOptions>.Instance.FootstepsDuration);
        }
    }
}