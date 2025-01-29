using LaunchpadReloaded.Options.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Modifiers.Fun;

[RegisterModifier]
public sealed class GiantModifier : GameModifier
{
    public override string ModifierName => "Giant";
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<GameModifierOptions>.Instance.GiantChance;
    public override int GetAmountPerGame() => 1;
    public override bool IsModifierValidOn(RoleBehaviour role) => !role.Player.HasModifier<SmolModifier>();
    public override void OnActivate()
    {
        if (Player != null)
        {
            Player.MyPhysics.Speed *= 0.75f;
            Player.transform.localScale /= 0.7f;
        }
    }

    public override void OnDeactivate()
    {
        if (Player != null)
        {
            Player.MyPhysics.Speed /= 0.75f;
            Player.transform.localScale *= 0.7f;
        }
    }
}