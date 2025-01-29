﻿using LaunchpadReloaded.Options.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Modifiers.Fun;

[RegisterModifier]
public sealed class SmolModifier : GameModifier
{
    public override string ModifierName => "Smol";
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<GameModifierOptions>.Instance.SmolChance;
    public override int GetAmountPerGame() => 1;

    public override bool IsModifierValidOn(RoleBehaviour role) => !role.Player.HasModifier<GiantModifier>();

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