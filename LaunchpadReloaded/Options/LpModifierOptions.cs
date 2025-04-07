using MiraAPI.GameOptions;
 using MiraAPI.GameOptions.Attributes;
 using MiraAPI.GameOptions.OptionTypes;
 using MiraAPI.Utilities;
 
 namespace LaunchpadReloaded.Options.Modifiers;
 
 public class LpModifierOptions : AbstractOptionGroup
 {
     public override string GroupName => "Modifier Options";
     public override uint GroupPriority => 0;
     public override bool ShowInModifiersMenu => true;
 }