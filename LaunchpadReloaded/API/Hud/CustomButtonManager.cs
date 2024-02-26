using System;
using System.Collections.Generic;

namespace LaunchpadReloaded.API.Hud;

public static class CustomButtonManager
{
    public static readonly List<CustomActionButton> CustomButtons = new();

    public static void RegisterButton(Type buttonType)
    {
        if (!typeof(CustomActionButton).IsAssignableFrom(buttonType))
        {
            return;
        }
        
        CustomButtons.Add((CustomActionButton)Activator.CreateInstance(buttonType));
    }
}