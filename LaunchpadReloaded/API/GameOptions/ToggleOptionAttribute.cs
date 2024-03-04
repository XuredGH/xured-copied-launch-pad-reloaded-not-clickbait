using System;

namespace LaunchpadReloaded.API.GameOptions;

[AttributeUsage(AttributeTargets.Property)]
public class ToggleOptionAttribute : Attribute
{
    public string Name { get; }

    public ToggleOptionAttribute(string name)
    {
        Name = name;
    }
}