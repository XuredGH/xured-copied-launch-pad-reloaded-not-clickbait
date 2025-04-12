using LaunchpadReloaded.Options.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using UnityEngine;

namespace LaunchpadReloaded.Modifiers.Fun;

public sealed class BabyModifier : LPModifier
{
    public override string ModifierName => "Tall";
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<GameModifierOptions>.Instance.SmolChance;
    public override int GetAmountPerGame() => 1;
    public override bool IsModifierValidOn(RoleBehaviour role) => base.IsModifierValidOn(role) && !role.Player.HasModifier<SmolModifier>() && !role.Player.HasModifier<ShortModifier>() && !role.Player.HasModifier<HumongousModifier>();

    public override void OnActivate()
    {
        if (Player != null)
        {
            Vector3 scale = Player.transform.localScale;
            scale.x *= 0.5f;
            scale.y *= 2f;
            Player.transform.localScale = scale;
        }
    }

    public override void OnDeactivate()
    {
        if (Player != null)
        {
            Vector3 scale = Player.transform.localScale;
            scale.x /= 0.5f;
            scale.y /= 2f;
            Player.transform.localScale = scale;
        }
    }
}