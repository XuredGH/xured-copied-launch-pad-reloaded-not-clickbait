using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using System.Collections.Generic;

namespace LaunchpadReloaded.Modifiers;

public class VoteData : BaseModifier
{
    public override string ModifierName => "Vote Data";
    public override bool HideOnUi => true;

    public readonly List<byte> VotedPlayers = [];
    public int VotesRemaining = (int)OptionGroupSingleton<VotingOptions>.Instance.MaxVotes.Value;

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }
}