using AmongUs.Data.Player;
using Il2CppInterop.Runtime;
using Il2CppSystem.Text.Json;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static GameData;
using static UnityEngine.GraphicsBuffer;

namespace LaunchpadReloaded.Buttons;
public class MapButton : CustomActionButton
{
    public override string Name => "Map";
    public override float Cooldown => 10;
    public override float EffectDuration => 3;
    public override int MaxUses => 0;
    public override Sprite Sprite => SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Map.png");

    private MapOptions _mapOptions = new MapOptions()
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