using Il2CppSystem.Runtime.Remoting.Messaging;
using LaunchpadReloaded.API.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;
public class CustomOptionGroup
{
    public string Title { get; }
    public Func<bool> Hidden { get; set; }

    public GameObject Header;
    public Type AdvancedRole { get; set; }

    public readonly List<AbstractGameOption> Options = new();
    public readonly List<CustomNumberOption> CustomNumberOptions = new();
    public readonly List<CustomToggleOption> CustomToggleOptions = new();
    public readonly List<CustomStringOption> CustomStringOptions = new();
    public CustomOptionGroup(string title, List<CustomNumberOption> numberOpt, 
        List<CustomToggleOption> toggleOpt, List<CustomStringOption> stringOpt, Type role = null)
    {
        Title = title; 
        
        if (role is not null && role.IsAssignableTo(typeof(ICustomRole)))
        {
            AdvancedRole = role;
        }


        Hidden = () => false;
        CustomNumberOptions = numberOpt;
        CustomToggleOptions = toggleOpt;
        CustomStringOptions = stringOpt;

        Options.AddRange(CustomNumberOptions);
        Options.AddRange(CustomToggleOptions);
        Options.AddRange(CustomStringOptions);

        foreach(AbstractGameOption option in Options)
        {
            Debug.Log(option.Title);
            option.Group = this;
        }

        CustomOptionsManager.CustomGroups.Add(this);
    }
}