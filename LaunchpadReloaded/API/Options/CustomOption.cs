using Reactor.Utilities.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace LaunchpadReloaded.API.Options;

public abstract class CustomOption
{
    public abstract string Text { get; }
    public StringNames StringName { get; set; }
    public abstract string Id { get; }
    public bool Enabled = false;
    private GameObject newButton;

    public GameObject CreateButton(OptionsMenuBehaviour optionsMenu, Transform parent)
    {
        var generalTab = optionsMenu.transform.FindChild("GeneralTab");
        var button = generalTab.FindChild("ChatGroup").FindChild("CensorChatButton");
        newButton = GameObject.Instantiate(button, parent).gameObject;
        newButton.name = this.Id;
        
        var tb = newButton.GetComponent<ToggleButtonBehaviour>();
        tb.BaseText = StringName;
        tb.UpdateText(Enabled);
        
        var pb = newButton.GetComponent<PassiveButton>();
        pb.OnClick.RemoveAllListeners();
        pb.OnClick.AddListener((UnityAction)ClickHandler);

        return newButton.gameObject;
    }

    private void ClickHandler()
    {
        Enabled = !Enabled;
        newButton.GetComponent<ToggleButtonBehaviour>().UpdateText(Enabled);
        OnClick();
    }

    public abstract void OnClick();
}