using LaunchpadReloaded.API.Hud;

namespace LaunchpadReloaded.Buttons;

public class DragButton : CustomActionButton
{
    public override string Name => "DRAG";
    public override float Cooldown => 0;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override string SpritePath => "Drag.png";

    private bool _isDragging;
    
    protected override void OnClick()
    {
        _isDragging = !_isDragging;
        if (_isDragging)
        {
            OverrideName("DROP");
            OverrideSprite("Drop.png");
        }
        else
        {
            OverrideName("DRAG");
            OverrideSprite("Drag.png");
        }
    }
}