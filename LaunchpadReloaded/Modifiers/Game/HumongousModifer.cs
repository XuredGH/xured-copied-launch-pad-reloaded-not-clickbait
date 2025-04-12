using LaunchpadReloaded.Options.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;

namespace LaunchpadReloaded.Modifiers.Fun;

public sealed class HumongousModifier : LPModifier
{
    public override string ModifierName => "Humongous";
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<GameModifierOptions>.Instance.GiantChance;
    public override int GetAmountPerGame() => 1;
    public override bool IsModifierValidOn(RoleBehaviour role) => base.IsModifierValidOn(role) && !role.Player.HasModifier<SmolModifier>() && !role.Player.HasModifier<BabyModifier>() && !role.Player.HasModifier<ShortModifier>();
    public override void OnActivate()
    {
        if (Player != null)
        {
            Player.MyPhysics.Speed *= 0.5f;
            Player.transform.localScale /= 0.5f;
        }
    }

    public override void OnDeactivate()
    {
        if (Player != null)
        {
            Player.MyPhysics.Speed /= 0.5f;
            Player.transform.localScale *= 0.5f;
        }
    }
}