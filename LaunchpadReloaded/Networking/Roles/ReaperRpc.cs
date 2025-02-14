using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles.Neutral;
using LaunchpadReloaded.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;

namespace LaunchpadReloaded.Networking.Roles;
public static class ReaperRpc
{
    [MethodRpc((uint)LaunchpadRpc.ReaperCollect)]
    public static void RpcCollectSoul(this PlayerControl playerControl, byte deadBody)
    {
        if (playerControl.Data.Role is not ReaperRole reaper)
        {
            playerControl.KickForCheating();
            return;
        }

        var body = Helpers.GetBodyById(deadBody);
        if (body != null)
        {
            body.gameObject.AddComponent<ReapedBodyComponent>();

            reaper.CollectedSouls += 1;

            if (reaper.CollectedSouls == OptionGroupSingleton<ReaperOptions>.Instance.SoulCollections
                && (AmongUsClient.Instance.AmHost || TutorialManager.InstanceExists))
            {
                GameManager.Instance.RpcEndGame((GameOverReason)GameOverReasons.ReaperWins, false);
            }
        }
    }
}