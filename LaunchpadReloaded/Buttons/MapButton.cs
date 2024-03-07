using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;
public class MapButton : CustomActionButton
{
    public override string Name => "Map";
    public override float Cooldown => 10;
    public override float EffectDuration => 3;
    public override int MaxUses => 0;
    public override Sprite Sprite => LaunchpadAssets.MapButton;

    private MapOptions _mapOptions = new()
    {
        IncludeDeadBodies = true,
        AllowMovementWhileMapOpen = true,
        ShowLivePlayerPosition = true,
        Mode = MapOptions.Modes.CountOverlay,
    };

    public override bool Enabled(RoleBehaviour role) =>  role is HackerRole;

    protected override void OnClick()
    {
        HudManager.Instance.ToggleMapVisible(_mapOptions);
    }

    protected override void OnEffectEnd()
    {
        base.OnEffectEnd();
        if (MapBehaviour.Instance.IsOpen) HudManager.Instance.ToggleMapVisible(null);
    }
}