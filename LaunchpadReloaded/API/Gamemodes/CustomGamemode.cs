﻿using System.Collections.Generic;

namespace LaunchpadReloaded.API.Gamemodes;
public abstract class CustomGamemode
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Id { get; }
    public virtual void Initialize() { }
    public virtual void HudStart(HudManager instance) { }
    public virtual void HudUpdate(HudManager instance) { }
    public virtual void OnDeath(PlayerControl player) { }
    public virtual void CheckGameEnd(out bool runOriginal, LogicGameFlowNormal instance)
    {
        runOriginal = true;
    }
    public virtual void AssignRoles(out bool runOriginal, LogicRoleSelectionNormal instance)
    {
        runOriginal = true;
    }

    public virtual void CanKill(out bool runOriginal, out bool result, PlayerControl target)
    {
        result = false;
        runOriginal = true;
    }

    public virtual bool CanAccessRolesTab() => true;
    public virtual bool CanAccessSettingsTab() => true;
    public virtual List<GameData.PlayerInfo> CalculateWinners() => null;
    public virtual bool ShowCustomRoleScreen() => false;
    public virtual bool CanUseMapConsole(MapConsole console) => true;
    public virtual bool CanReport(DeadBody body) => true;
    public virtual bool CanUseSystemConsole(SystemConsole console) => true;
    public virtual bool CanUseConsole(Console console) => true;
    public virtual bool ShouldShowSabotageMap(MapBehaviour map) => true;
    public virtual bool CanVent(Vent vent, GameData.PlayerInfo playerInfo) => true;
}