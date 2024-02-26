﻿using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using BepInEx.Configuration;
using Il2CppInterop.Runtime;
using LaunchpadReloaded.Utilities;
using Reactor.Localization.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;
using Object = System.Object;

namespace LaunchpadReloaded.API.Roles;

public static class CustomRoleManager
{
    public static readonly Dictionary<ushort, RoleBehaviour> CustomRoles = [];
    
    public static void RegisterInRoleManager()
    {
        RoleManager.Instance.AllRoles = RoleManager.Instance.AllRoles.Concat(CustomRoles.Values).ToArray();
    }
    
    public static void RegisterRole(Type roleType)
    {
        if (!(typeof(RoleBehaviour).IsAssignableFrom(roleType) && typeof(ICustomRole).IsAssignableFrom(roleType)))
        {
            return;
        }
        
        if (CustomRoles.Any(behaviour => behaviour.GetType() == roleType))
        {
            return;
        }

        var roleBehaviour = (RoleBehaviour) new GameObject(roleType.Name).DontDestroy().AddComponent(Il2CppType.From(roleType));

        if (roleBehaviour is ICustomRole customRole)
        {
            var roleId = (ushort)(Enum.GetNames<RoleTypes>().Length + CustomRoles.Count);
            roleBehaviour.Role = (RoleTypes)roleId;
            roleBehaviour.TeamType = customRole.Team;
            roleBehaviour.NameColor = customRole.RoleColor;
            roleBehaviour.StringName = CustomStringName.CreateAndRegister(customRole.RoleName);
            roleBehaviour.BlurbName = CustomStringName.CreateAndRegister(customRole.RoleDescription);
            roleBehaviour.BlurbNameLong = CustomStringName.CreateAndRegister(customRole.RoleLongDescription);
            roleBehaviour.AffectedByLightAffectors = customRole.AffectedByLight;
            roleBehaviour.CanBeKilled = customRole.CanGetKilled;
            roleBehaviour.CanUseKillButton = customRole.CanUseKill;
            roleBehaviour.CanVent = customRole.CanUseVent;
            roleBehaviour.DefaultGhostRole = customRole.GhostRole;
            roleBehaviour.MaxCount = 15;
            CustomRoles.Add(roleId,roleBehaviour);

            var config = PluginSingleton<LaunchpadReloadedPlugin>.Instance.Config;
            config.Bind(customRole.NumConfigDefinition,1);
            config.Bind(customRole.ChanceConfigDefinition, 100);
        }
        
    }
    
    public static bool GetCustomRoleBehaviour(RoleTypes roleType, out ICustomRole result)
    {
        CustomRoles.TryGetValue((ushort)roleType, out var temp );
        if (temp is ICustomRole role)
        {
            result = role;
            return true;
        }

        result = null;
        return false;
    }
}