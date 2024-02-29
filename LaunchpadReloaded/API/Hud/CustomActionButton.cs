using Reactor.Utilities.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LaunchpadReloaded.API.Hud;

public abstract class CustomActionButton
{
    public abstract string Name { get; }
    
    public abstract float Cooldown { get; }
    
    public abstract float EffectDuration { get; }
    
    public abstract int MaxUses { get; }
    
    public abstract string SpritePath { get; }
    
    public bool HasEffect => EffectDuration > 0;
    
    public bool LimitedUses => MaxUses > 0;

    protected bool EffectActive;
    
    protected float Timer;
    
    protected int UsesLeft;
    
    protected ActionButton Button;

    public void CreateButton(Transform parent)
    {
        if (Button)
        {
            return;
        }

        UsesLeft = MaxUses;
        Timer = 0;
        EffectActive = false;
        
        Button = Object.Instantiate(HudManager.Instance.AbilityButton, parent);
        Button.name = Name + "Button";
        Button.OverrideText(Name);
        
        Button.graphic.sprite = LaunchpadReloadedPlugin.Bundle.LoadAsset<Sprite>(SpritePath);

        Button.SetUsesRemaining(MaxUses);
        if (MaxUses <= 0)
        {
            Button.SetInfiniteUses();
        }

        var pb = Button.GetComponent<PassiveButton>();
        pb.OnClick.RemoveAllListeners();
        pb.OnClick.AddListener((UnityAction)ClickHandler);
    }

    public void OverrideSprite(string path)
    {
        Button.graphic.sprite = LaunchpadReloadedPlugin.Bundle.LoadAsset<Sprite>(path);
    }

    public void OverrideName(string name)
    {
        Button.OverrideText(name);
    }
    
    protected virtual void FixedUpdate(PlayerControl playerControl) { }

    protected abstract void OnClick();
    
    public abstract bool Enabled(RoleBehaviour role);
    
    protected virtual void OnEffectEnd() { }

    public virtual bool CanUse()
    {
        return Timer <= 0 && !EffectActive && (!LimitedUses || MaxUses > 0);
    }
    
    public virtual void SetActive(bool visible, RoleBehaviour role)
    {
        Button.ToggleVisible(visible && Enabled(role));
    }
    
    private void ClickHandler()
    {
        if (!CanUse())
        {
            return;
        }

        if (LimitedUses)
        {
            UsesLeft--;
            Button.SetUsesRemaining(UsesLeft);
        }
        
        OnClick();
        Button.SetDisabled();
        if (HasEffect)
        {
            EffectActive = true;
            Timer = EffectDuration;
        }
        else
        {
            Timer = Cooldown;
        }
    }
    
    public void UpdateHandler(PlayerControl playerControl)
    {
        if (Timer >= 0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            if (HasEffect && EffectActive)
            {
                EffectEndHandler();
            }
        }

        if (CanUse())
        {
            Button.SetEnabled();
        }
        else
        {
            Button.SetDisabled();
        }
        Button.SetCoolDown(Timer, EffectActive ? EffectDuration : Cooldown);
        
        FixedUpdate(playerControl);
    }

    private void EffectEndHandler()
    {
        EffectActive = false;
        Timer = Cooldown;
        OnEffectEnd();
    }
}