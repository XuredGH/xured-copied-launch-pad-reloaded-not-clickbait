namespace LaunchpadReloaded.Features;
public struct CustomVote
{
    public byte Voter;
    public byte VotedFor;

    public CustomVote(byte voter, byte votedFor)
    {
        this.Voter = voter;
        this.VotedFor = votedFor;
    }
}