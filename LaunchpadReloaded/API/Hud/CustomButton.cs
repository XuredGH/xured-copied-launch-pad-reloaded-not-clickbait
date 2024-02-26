using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using BepInEx.Configuration;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.API.Hud;

public class CustomButton
{
    public static readonly List<CustomButton> AllButtons = new ();
    
    public readonly string Name;
    public ConfigEntry<float> Cooldown;
    public float Timer;
    public ConfigEntry<int> MaxUses;
    public int UsesLeft;
    public ConfigEntry<float> EffectDuration;
    public readonly bool HasEffect;
    public bool IsEffectActive;
    public readonly string ResourcePath;

    public ActionButton Button;
    
    public readonly Action OnClick;
    public readonly Action OnEnd;

    
    public RoleTypes[] RoleTypes = { };

    public CustomButton(Action onClick, Action onEnd, string name, float cooldown, float duration, string resourcePath, int maxUses = 0)
    {
        OnClick = onClick;
        OnEnd = onEnd;
        Name = name;
        Cooldown = PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.Bind(
            new ConfigDefinition("Buttons", name + ".Cooldown"), cooldown,
            new ConfigDescription($"Cooldown amount in seconds for {name} button"));
        Timer = Cooldown.Value;
        EffectDuration = PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.Bind(
            new ConfigDefinition("Buttons", name + ".Duration"), duration,
            new ConfigDescription($"Duration of effect for {name} button"));;
        HasEffect = true;
        IsEffectActive = false;
        ResourcePath = resourcePath;
        MaxUses = PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.Bind(
            new ConfigDefinition("Buttons", name + ".MaxUses"), maxUses,
            new ConfigDescription($"Max number of uses per game for {name} button"));
        UsesLeft = MaxUses.Value;
        AllButtons.Add(this);
    }

    public CustomButton(Action onClick, string name, float cooldown, string resourcePath, int maxUses = 0)
    {
        OnClick = onClick;
        Name = name;
        Cooldown = PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.Bind(
            new ConfigDefinition("Buttons", name + ".Cooldown"), cooldown,
            new ConfigDescription($"Cooldown amount in seconds for {name} button"));
        Timer = Cooldown.Value;
        HasEffect = false;
        ResourcePath = resourcePath;
        MaxUses = PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.Bind(
            new ConfigDefinition("Buttons", name + ".MaxUses"), maxUses,
            new ConfigDescription($"Max number of uses per game for {name} button"));
        UsesLeft = MaxUses.Value;
        AllButtons.Add(this);
    }
    
    public void VerifyButton()
    {
        if (Button) return;
        Button = Object.Instantiate(HudManager.Instance.AbilityButton, HudManager.Instance.AbilityButton.transform.parent);
        Button.SetCoolDown(0, Cooldown.Value);
        
        if (ResourcePath != null)
        {
            Button.graphic.sprite = SpriteTools.LoadSpriteFromPath(ResourcePath);
        }

        Button.SetUsesRemaining(MaxUses.Value);
        if (MaxUses.Value <= 0)
        {
            Button.SetInfiniteUses();
        }
        
        var pb = Button.GetComponent<PassiveButton>();
        pb.OnClick.RemoveAllListeners();
        pb.OnClick.AddListener((UnityAction)Click);
        
        Button.Hide();
    }
    
    private void Click()
    {
        if (CanUse())
        {
            Timer = Cooldown.Value;
            if (HasEffect)
            {
                IsEffectActive = true;
                Timer = EffectDuration.Value;
            }

            if (MaxUses.Value > 0)
            {
                UsesLeft--;
                Button.SetUsesRemaining(UsesLeft);
                if (UsesLeft == 0)
                {
                    Button.gameObject.SetActive(false);
                }
            }

            Button.SetDisabled();
            OnClick();
        }
    }
    
    public bool CanUse()
    {
        return Timer < 0f && (MaxUses.Value <= 0 || UsesLeft > 0) && !IsEffectActive;
    }

    public static void UpdateButtons()
    {
        foreach (var button in AllButtons)
        {
            button.Update();
        }
    }

    public void SetHudActive(HudManager hudManager, RoleBehaviour role, bool isActive)
    {
        VerifyButton();
        
        var flag = isActive;
        if (RoleTypes.Length > 0)
        {
            flag = isActive && RoleTypes.Contains(role.Role);
        }
        Button.ToggleVisible(flag);
    }
    
    private void Update()
    {
        VerifyButton();
        
        Button.OverrideText(Name); // poor fix, but until i figure out why putting this in Start() doesnt work, it stays
        if (Timer < 0f)
        {
            Button.SetEnabled();
            if (IsEffectActive)
            {
                Timer = Cooldown.Value;
                IsEffectActive = false;
                OnEnd();
            }
        }
        else
        {
            Button.SetDisabled();
            try
            {
                if (PlayerControl.LocalPlayer.ButtonTimerEnabled())
                    Timer -= Time.deltaTime;
            }
            catch
            {
                Timer -= Time.deltaTime;
            }
        }
        Button.SetCoolDown(Timer, Cooldown.Value);
    }
}