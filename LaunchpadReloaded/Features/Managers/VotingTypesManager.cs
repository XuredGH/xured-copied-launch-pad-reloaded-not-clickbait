using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;

namespace LaunchpadReloaded.Features.Managers;
public static class VotingTypesManager
{
    public static VotingTypes SelectedType = VotingTypes.Classic;

    public static void SetType(VotingTypes type)
    {
        SelectedType = type;
    }

    public static int GetVotes()
    {
        switch (SelectedType)
        {
            case VotingTypes.Multiple:
            case VotingTypes.Combined:
                return (int)LaunchpadGameOptions.Instance.MaxVotes.Value;

            case VotingTypes.Chance:
            case VotingTypes.Classic:
            default:
                return 1;
        }
    }

    [MethodRpc((uint)LaunchpadRpc.SetVotingType)]
    public static void RpcSetType(GameData lobby, int type)
    {
        SetType((VotingTypes)type);
    }

    public static bool CanVoteMultiple() => SelectedType is VotingTypes.Multiple or VotingTypes.Combined;
}