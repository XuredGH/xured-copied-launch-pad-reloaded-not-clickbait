using System;
 using LaunchpadReloaded.Modifiers;
 using MiraAPI.GameOptions;
 using MiraAPI.GameOptions.Attributes;
 using MiraAPI.Utilities;
 
 namespace LaunchpadReloaded.Options.Modifiers;
 
 public class VendettaOptions : AbstractOptionGroup<VendettaMarkModifier>
 {
     public override string GroupName => "Vendetta";
 
     public override Func<bool> GroupVisible =>
         () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.VendettaChance > 0;
 
     [ModdedNumberOption("Mark Cooldown", 5, 40, 2.5f, MiraNumberSuffixes.Seconds)]
     public float MarkCooldown { get; set; } = 15;
     
     [ModdedNumberOption("Marks Per Round", 1, 3, 1f)]
     public float MarkUses { get; set; } = 1;
 }