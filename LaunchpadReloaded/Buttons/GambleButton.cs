using Il2CppSystem;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Networking;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

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

        GuessRoleMenu playerMenu = GuessRoleMenu.Create();
        playerMenu.Begin((role) => !role.IsDead, (selectedRole) =>
        {
            if (selectedRole.Role != Target.Data.Role.Role)
            {
                Target.RpcCustomMurder(PlayerControl.LocalPlayer, resetKillTimer: false, teleportMurderer: false, showKillAnim: false, playKillSound: false);
            }
            else
            {
                Target.RpcAddModifier<RevealedModifier>();
            }

            playerMenu.Close();

            ResetCooldownAndOrEffect();
        });

        ResetTarget();
    }
}