using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class MapButton : CustomActionButton
{
    public override string Name => "Map";
    public override float Cooldown => (int)ModdedGroupSingleton<HackerOptions>.Instance.MapCooldown;
    public override float EffectDuration => (int)ModdedGroupSingleton<HackerOptions>.Instance.MapDuration;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.MapButton;

    private readonly MapOptions _mapOptions = new()
    {
        IncludeDeadBodies = true,
        AllowMovementWhileMapOpen = true,
        ShowLivePlayerPosition = true,
        Mode = MapOptions.Modes.CountOverlay,
    };
    
    public override bool Enabled(RoleBehaviour role) => role is HackerRole;

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