using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Utilities;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using System.Collections;
using UnityEngine;

namespace LaunchpadReloaded.Modifiers;

[RegisterModifier]
public class FootstepsModifier : BaseModifier
{
    public override string ModifierName => "Footsteps";

    public override bool HideOnUi => true;

    private Vector3 _lastPos;

    public override void OnActivate()
    {
        _lastPos = Player!.transform.position;
    }

    public IEnumerator FootstepDisappear(GameObject obj, SpriteRenderer rend)
    {
        yield return new WaitForSeconds(OptionGroupSingleton<DetectiveOptions>.Instance.FootstepsDuration);
        yield return Helpers.FadeOut(rend, 0.0001f, 0.05f);
        obj.DestroyImmediate();
    }

    private bool _lastFlip = false;

    public override void FixedUpdate()
    {
        if (Vector3.Distance(_lastPos, Player!.transform.position) < 1)
        {
            return;
        }

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

        if (_lastFlip == false)
        {
            _lastFlip = true;
            sprite.flipX = true;
        }
        else
        {
            _lastFlip = false;
            sprite.flipX = false;
        }

        sprite.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
        Player.SetPlayerMaterialColors(sprite);

        _lastPos = Player.transform.position;
        Coroutines.Start(FootstepDisappear(footstep, sprite));
    }
}