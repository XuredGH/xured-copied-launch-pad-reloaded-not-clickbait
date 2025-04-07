using LaunchpadReloaded.Options.Modifiers;
 using LaunchpadReloaded.Options.Modifiers.Crewmate;
 using MiraAPI.GameOptions;
 using MiraAPI.Utilities;
 
 namespace LaunchpadReloaded.Modifiers;
 
 public sealed class TorchModifier : LPModifier
 {
     public override string ModifierName => "Torch";
     public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.TorchChance;
     public override int GetAmountPerGame() => 1;
 }