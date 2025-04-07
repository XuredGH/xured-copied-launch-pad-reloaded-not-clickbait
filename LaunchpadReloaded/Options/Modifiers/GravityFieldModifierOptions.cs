using System;
 using LaunchpadReloaded.Modifiers.Fun;
 using MiraAPI.GameOptions;
 using MiraAPI.GameOptions.Attributes;
 using MiraAPI.Utilities;
 
 namespace LaunchpadReloaded.Options.Modifiers;
 
 public class GravityFieldOptions : AbstractOptionGroup<GravityModifier>
 {
     public override string GroupName => "Gravity Field";
 
     public override Func<bool> GroupVisible =>
         () => OptionGroupSingleton<GameModifierOptions>.Instance.GravityChance > 0;
 
     [ModdedNumberOption("Gravity Field Radius", 0.5f, 10f, 0.5f, suffixType: MiraNumberSuffixes.None)]
     public float GravityFieldRadius { get; set; } = 2f;
 }