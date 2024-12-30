using System.Collections.Generic;
using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;

namespace LaunchpadReloaded.Modifiers;

[RegisterModifier]
public class VoteData : BaseModifier
{
    public override string ModifierName => "Vote Data";
    public override bool HideOnUi => true;

    public readonly List<byte> VotedPlayers = [];
    public int VotesRemaining = (int)OptionGroupSingleton<VotingOptions>.Instance.MaxVotes.Value;
}