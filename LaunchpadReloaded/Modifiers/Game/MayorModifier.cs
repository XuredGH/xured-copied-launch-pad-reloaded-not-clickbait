﻿using LaunchpadReloaded.Options.Modifiers;
 using LaunchpadReloaded.Options.Modifiers.Crewmate;
 using MiraAPI.GameOptions;
 using MiraAPI.Utilities;
 
 namespace LaunchpadReloaded.Modifiers;
 
 public sealed class MayorModifier : LPModifier
 {
     public override string ModifierName => "Mayor";
     public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.MayorChance;
     public override int GetAmountPerGame() => 1;
 
     public override void OnMeetingStart()
     {
         var voteData = Player.GetVoteData();
         if (!voteData) return;
 
         voteData.VotesRemaining += (int)OptionGroupSingleton<MayorOptions>.Instance.ExtraVotes;
     }
 }