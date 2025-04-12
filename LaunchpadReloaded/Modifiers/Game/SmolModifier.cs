﻿using LaunchpadReloaded.Options.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;

namespace LaunchpadReloaded.Modifiers.Fun;

public sealed class SmolModifier : LPModifier
{
    public override string ModifierName => "Smol";
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<GameModifierOptions>.Instance.SmolChance;
    public override int GetAmountPerGame() => 1;
    public override bool IsModifierValidOn(RoleBehaviour role) => base.IsModifierValidOn(role) && !role.Player.HasModifier<HumongousModifier>() && !role.Player.HasModifier<BabyModifier>() && !role.Player.HasModifier<ShortModifier>();

    public override void OnActivate()
    {
        if (Player != null)
        {
            Player.MyPhysics.Speed /= 0.75f;
            Player.transform.localScale *= 0.7f;
        }
    }

    public override void OnDeactivate()
    {
        if (Player != null)
        {
            Player.MyPhysics.Speed *= 0.75f;
            Player.transform.localScale /= 0.7f;
        }
    }
}