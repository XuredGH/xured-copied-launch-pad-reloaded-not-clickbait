using LaunchpadReloaded.Options.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities;
using System.Linq;

namespace LaunchpadReloaded.Modifiers;

public abstract class LPModifier : GameModifier
{
    public override int GetAmountPerGame() => 1;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        if (OptionGroupSingleton<GameModifierOptions>.Instance.ModifierLimit == 0) return true;

        return role.Player.GetModifierComponent()!.ActiveModifiers.OfType<LPModifier>().Count() < OptionGroupSingleton<GameModifierOptions>.Instance.ModifierLimit;
    }
}