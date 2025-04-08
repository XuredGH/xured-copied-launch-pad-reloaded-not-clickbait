using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles.Neutral;
using MiraAPI.GameEnd;
using MiraAPI.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.GameOver;

public sealed class ChickenGameOver : CustomGameOver
{
    public override bool VerifyCondition(PlayerControl playerControl, NetworkedPlayerInfo[] winners)
    {
        return winners is [{ Role: ChickenRole }];
    }

    public override void AfterEndGameSetup(EndGameManager endGameManager)
    {
        endGameManager.WinText.text = "Chicken Wins!\n<size=1>ba-cawk!";
        endGameManager.WinText.color = LaunchpadPalette.ChickenColor;
        endGameManager.BackgroundBar.material.SetColor(ShaderID.Color, LaunchpadPalette.ChickenColor);
    }
}