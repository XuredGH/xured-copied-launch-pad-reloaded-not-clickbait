using AmongUs.GameOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LaunchpadReloaded.API.Gamemodes.CustomGamemodeManager;

namespace LaunchpadReloaded.API.Gamemodes;
public abstract class CustomGamemode
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Id { get; }
    public virtual void Initialize() { }
    public virtual void HudStart(HudManager instance) { }
    public virtual void HudUpdate(HudManager instance) { }
    public virtual void CheckGameEnd(out bool runOriginal, LogicGameFlowNormal instance)
    {
        runOriginal = true;
    }
    public virtual void AssignRoles(out bool runOriginal, LogicRoleSelectionNormal instance)
    {
        runOriginal = true;
    }
    public virtual List<GameData.PlayerInfo> CalculateWinners() => null;
    public virtual bool ShowCustomRoleScreen() => false;
    public virtual bool CanUseMapConsole(MapConsole console) => true;
    public virtual bool CanReport(DeadBody body) => true;
    public virtual bool CanKill(PlayerControl target) => true;
    public virtual bool CanUseSystemConsole(SystemConsole console) => true;
    public virtual bool CanUseConsole(Console console) => true;
    public virtual bool ShouldShowSabotageMap(MapBehaviour map) => true;
    public virtual bool CanVent(Vent vent, GameData.PlayerInfo playerInfo) => true;
}