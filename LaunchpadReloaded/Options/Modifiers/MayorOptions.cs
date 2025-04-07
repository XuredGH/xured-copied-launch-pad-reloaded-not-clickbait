using System;
 using LaunchpadReloaded.Modifiers;
 using MiraAPI.GameOptions;
 using MiraAPI.GameOptions.Attributes;
 using MiraAPI.Utilities;
 
 namespace LaunchpadReloaded.Options.Modifiers;
 
 public class MayorOptions : AbstractOptionGroup<MayorModifier>
 {
     public override string GroupName => "Mayor";
 
     public override Func<bool> GroupVisible =>
         () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.MayorChance > 0;
 
     [ModdedNumberOption("Extra Votes", 1, 3)]
     public float ExtraVotes { get; set; } = 1;
 
     [ModdedToggleOption("Allow Multiple Votes on Same Player")]
     public bool AllowVotingTwice { get; set; } = true;
 }