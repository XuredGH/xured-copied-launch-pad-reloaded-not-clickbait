using LaunchpadReloaded.Options.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using UnityEngine;

namespace LaunchpadReloaded.Modifiers.Fun;

public sealed class ShortModifier : LPModifier
{
    public override string ModifierName => "Short";
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<GameModifierOptions>.Instance.GiantChance;
    public override int GetAmountPerGame() => 1;
    public override bool IsModifierValidOn(RoleBehaviour role) => base.IsModifierValidOn(role) && !role.Player.HasModifier<SmolModifier>() && !role.Player.HasModifier<BabyModifier>() && !role.Player.HasModifier<HumongousModifier>();
    public override void OnActivate()
    {
        if (Player != null)
        {
            Vector3 scale = Player.transform.localScale;
            scale.x *= 2f;
            scale.y *= 0.5f;
            Player.transform.localScale = scale;
        }
    }

    public override void OnDeactivate()
    {
        if (Player != null)
        {
            Vector3 scale = Player.transform.localScale;
            scale.x /= 2f;
            scale.y /= 0.5f;
            Player.transform.localScale = scale;
        }
    }
}