using AmongUs.Data;
using HarmonyLib;
using LaunchpadReloaded.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameData;

namespace LaunchpadReloaded.API.Gamemodes;
public class DefaultMode : CustomGamemode
{
    public override string Name => "Default";
    public override string Description => "Default Among Us Gamemode";

    public override int Id => 0;

    public override void Begin()
    {

    }
}