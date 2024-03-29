using Reactor.Localization.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace LaunchpadReloaded.API.Settings;

public class CustomSetting
{
    public string Text { get; }
    public StringNames StringName { get; set; }
    public bool Enabled;
    
    private GameObject _buttonObject;

    public CustomSetting(string name)
    {
        Text = name;
        StringName = CustomStringName.CreateAndRegister(name);
        
        CustomSettingsManager.CustomSettings.Add(this);
    }
    
    
    public GameObject CreateButton(OptionsMenuBehaviour optionsMenu, Transform parent)
    {
        var generalTab = optionsMenu.transform.FindChild("GeneralTab");
        var button = generalTab.FindChild("ChatGroup").FindChild("CensorChatButton");
        _buttonObject = Object.Instantiate(button, parent).gameObject;
        _buttonObject.name = Text;
        
        var tb = _buttonObject.GetComponent<ToggleButtonBehaviour>();
        tb.BaseText = StringName;
        tb.UpdateText(Enabled);
        
        var pb = _buttonObject.GetComponent<PassiveButton>();
        pb.OnClick.RemoveAllListeners();
        pb.OnClick.AddListener((UnityAction)ClickHandler);

        return _buttonObject.gameObject;
    }

    private void ClickHandler()
    {
        Enabled = !Enabled;
        _buttonObject.GetComponent<ToggleButtonBehaviour>().UpdateText(Enabled);
        OnClick();
    }

    protected virtual void OnClick() {}
}