using Il2CppSystem;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Options.Roles.Crewmate;
using LaunchpadReloaded.Roles.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.Networking;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons.Crewmate;

public class GambleButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Gamble";
    public override float Cooldown => OptionGroupSingleton<GamblerOptions>.Instance.GambleCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)OptionGroupSingleton<GamblerOptions>.Instance.GambleUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.GambleButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => true;

    public override bool Enabled(RoleBehaviour? role) => role is GamblerRole;

    public override PlayerControl? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestPlayer(true,
            GameOptionsManager.Instance.normalGameHostOptions.KillDistance);
    }

    public override bool CanUse()
    {
        return base.CanUse() && !Target.HasModifier<RevealedModifier>();
    }
    public override void SetOutline(bool active)
    {
        Target?.cosmetics.SetOutline(active, new Nullable<Color>(LaunchpadPalette.GamblerColor));
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }

        GuessRoleMinigame playerMenu = GuessRoleMinigame.Create();
        playerMenu.Open(role => !role.IsDead, selectedRole =>
        {
            if (selectedRole.Role != Target.Data.Role.Role)
            {
                PlayerControl.LocalPlayer.RpcCustomMurder(PlayerControl.LocalPlayer, resetKillTimer: false, teleportMurderer: false, showKillAnim: false, playKillSound: true);
            }
            else
            {
                SoundManager.Instance.PlaySound(LaunchpadAssets.MoneySound.LoadAsset(), false, volume: 5);
                Target.RpcAddModifier<RevealedModifier>();
            }

            playerMenu.Close();

            ResetCooldownAndOrEffect();
        });

        ResetTarget();
    }
}