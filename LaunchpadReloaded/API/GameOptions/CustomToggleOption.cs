using Reactor.Localization.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomToggleOption : CustomOption
{
    public bool Value { get; private set; }
    
    public bool OldValue { get; private set; }
    
    public CustomToggleOption(string title, bool defaultValue) : base(title)
    {
        Value = defaultValue;
        
        CustomGameOptionsManager.CustomToggleOptions.Add(this);
    }

    public void SetValue(bool newValue)
    {
        OldValue = Value;
        Value = newValue;
    }
    
    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        OldValue = Value;
        Value = optionBehaviour.GetBool();
        Debug.LogError(OldValue+", "+Value);
    }
}