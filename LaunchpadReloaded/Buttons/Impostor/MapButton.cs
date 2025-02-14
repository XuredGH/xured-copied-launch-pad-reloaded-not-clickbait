using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles.Impostor;
using LaunchpadReloaded.Roles.Impostor;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons.Impostor;

public class MapButton : BaseLaunchpadButton
{
    public override string Name => "Map";
    public override float Cooldown => (int)OptionGroupSingleton<HackerOptions>.Instance.MapCooldown;
    public override float EffectDuration => (int)OptionGroupSingleton<HackerOptions>.Instance.MapDuration;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.MapButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => false;

    private readonly MapOptions _mapOptions = new()
    {
        IncludeDeadBodies = true,
        AllowMovementWhileMapOpen = true,
        ShowLivePlayerPosition = true,
        Mode = MapOptions.Modes.CountOverlay,
    };

    public override bool Enabled(RoleBehaviour? role) => role is HackerRole;

    protected override void OnClick()
    {
        HudManager.Instance.ToggleMapVisible(_mapOptions);
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
        if (MapBehaviour.Instance.IsOpen)
        {
            HudManager.Instance.ToggleMapVisible(null);
        }
    }
}