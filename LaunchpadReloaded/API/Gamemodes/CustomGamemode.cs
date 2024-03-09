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
    public abstract void Begin();
    public virtual void HudUpdate(HudManager instance) { }
    public virtual void CheckGameEnd(out bool runOriginal, LogicGameFlowNormal instance)
    {
        runOriginal = true;
    }
    public virtual void AssignRoles(out bool runOriginal, List<GameData.PlayerInfo> players, LogicRoleSelectionNormal instance)
    {
        runOriginal = true;
    }
}