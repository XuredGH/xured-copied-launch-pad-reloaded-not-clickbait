﻿using LaunchpadReloaded.Options.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using MiraAPI.PluginLoading;
using System.Linq;
using MiraAPI.Modifiers;

namespace LaunchpadReloaded.Modifiers;

[MiraIgnore]
public abstract class LPModifier : GameModifier
{
    public virtual bool RemoveOnDeath => true;
    public override int GetAmountPerGame() => 1;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return role.Player.GetModifierComponent().ActiveModifiers.OfType<LPModifier>().Count() < 1;
    }

    public override void OnDeath(DeathReason reason)
    {
        if (!RemoveOnDeath) return;
        ModifierComponent!.RemoveModifier(this);
    }
}