using System;
using System.Collections.Generic;
using System.Reflection;

namespace LaunchpadReloaded.API.Hud;

public static class CustomButtonManager
{
    public static readonly List<CustomActionButton> CustomButtons = new();

    public static void RegisterAllButtons()
    {
        foreach (var type in Assembly.GetCallingAssembly().GetTypes())
        {
            if (type.IsAssignableTo(typeof(CustomActionButton)) && !type.IsAbstract)
            {
                RegisterButton(type);
            }
        }
    }
    
    public static void RegisterButton(Type buttonType)
    {
        if (!typeof(CustomActionButton).IsAssignableFrom(buttonType))
        {
            return;
        }

        var button = (CustomActionButton)Activator.CreateInstance(buttonType);
        CustomButtons.Add(button);
    }
}