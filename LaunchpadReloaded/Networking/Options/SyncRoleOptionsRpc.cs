using System;
using Hazel;
using LaunchpadReloaded.API.Roles;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;

namespace LaunchpadReloaded.Networking.Options;

[RegisterCustomRpc((uint)LaunchpadRpc.SyncRoleOption)]
public class SyncRoleOptionsRpc(LaunchpadReloadedPlugin plugin, uint id) : PlayerCustomRpc<LaunchpadReloadedPlugin, SyncRoleOptionsRpc.Data>(plugin, id)
{

    public struct Data(ushort roleId, int number, int chance)
    {
        public readonly ushort RoleId = roleId;
        public readonly int Number = number;
        public readonly int Chance = chance;
    }

    public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;
    
    public override void Write(MessageWriter writer, Data data)
    {
        writer.Write(data.RoleId);
        writer.WritePacked(data.Number);
        writer.WritePacked(data.Chance);
    }

    public override Data Read(MessageReader reader)
    {
        return new Data(reader.ReadUInt16(), reader.ReadPackedInt32(), reader.ReadPackedInt32());
    }

    public override void Handle(PlayerControl playerControl, Data data)
    {
        if (AmongUsClient.Instance.HostId != playerControl.OwnerId)
        {
            return;
        }
        
        if (!CustomRoleManager.CustomRoles.TryGetValue(data.RoleId, out var roleBehaviour) ||
            !CustomRoleManager.GetCustomRoleBehaviour(roleBehaviour.Role, out var role)) return;
        
        PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.TryGetEntry<int>(role.NumConfigDefinition, out var numEntry);
        PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config.TryGetEntry<int>(role.ChanceConfigDefinition, out var chanceEntry);

        try
        {
            numEntry.Value = data.Number;
            chanceEntry.Value = data.Chance;
        }
        catch (Exception e)
        {
            Logger<LaunchpadReloadedPlugin>.Warning(e.ToString());
        }
    }
}