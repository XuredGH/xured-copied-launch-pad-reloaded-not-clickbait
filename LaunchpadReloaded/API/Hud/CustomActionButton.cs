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
    private bool HasEffect => EffectDuration > 0;
    private bool LimitedUses => MaxUses > 0;
    
    private bool _effectActive;
    protected float _timer;
    private int _usesLeft;
    protected ActionButton _button;

    public void CreateButton(Transform parent)
    {
        if (_button) return;

        _usesLeft = MaxUses;
        _timer = 0;
        _effectActive = false;
        
        _button = Object.Instantiate(HudManager.Instance.AbilityButton, parent);
        _button.name = Name + "Button";
        _button.OverrideText(Name);
        
        _button.graphic.sprite = LaunchpadReloadedPlugin.Bundle.LoadAsset<Sprite>(SpritePath);

        _button.SetUsesRemaining(MaxUses);
        if (MaxUses <= 0)
        {
            _button.SetInfiniteUses();
        }

        var pb = _button.GetComponent<PassiveButton>();
        pb.OnClick.RemoveAllListeners();
        pb.OnClick.AddListener((UnityAction)ClickHandler);
    }

    public virtual bool Enabled(RoleBehaviour role)
    {
        return true;
    }

    protected virtual bool CanUse()
    {
        return _timer <= 0 && !_effectActive;
    }

    public void OverrideSprite(string path)
    {
        _button.graphic.sprite = LaunchpadReloadedPlugin.Bundle.LoadAsset<Sprite>(path);
    }

    public void OverrideName(string name)
    {
        _button.OverrideText(name);
    }
    
    public virtual void SetActive(bool visible, RoleBehaviour role)
    {
        _button.ToggleVisible(visible && Enabled(role));
    }
    
    public virtual void Update(PlayerControl playerControl)
    {
        if (_timer >= 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            if (HasEffect && _effectActive)
            {
                OnEffectEnd();
            }
        }

        if (CanUse())
        {
            _button.SetEnabled();
        }
        else
        {
            _button.SetDisabled();
        }
        _button.SetCoolDown(_timer, _effectActive ? EffectDuration : Cooldown);
    }
    
    private void ClickHandler()
    {
        if (!CanUse()) return;

        if (LimitedUses)
        {
            _usesLeft--;
            _button.SetUsesRemaining(_usesLeft);
        }
        
        OnClick();
        _button.SetDisabled();
        if (HasEffect)
        {
            _effectActive = true;
            _timer = EffectDuration;
        }
        else
        {
            _timer = Cooldown;
        }
    }

    protected abstract void OnClick();
    
    protected virtual void OnEffectEnd()
    {
        _effectActive = false;
        _timer = Cooldown;
    }

}