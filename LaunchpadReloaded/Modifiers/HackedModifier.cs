using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Utilities;
using MiraAPI.Modifiers;
using Reactor.Utilities;

namespace LaunchpadReloaded.Modifiers;

public class HackedModifier : BaseModifier
{
    public override string ModifierName => "Hacked";
    
    public override void FixedUpdate()
    {
        var randomString = Helpers.RandomString(Helpers.Random.Next(4, 6),
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#!?$(???#@)$@@@@0000");
        Player!.cosmetics.SetName(randomString);
        Player.cosmetics.SetNameMask(true);
        Player.cosmetics.gameObject.SetActive(false);
    }

    public override void OnActivate()
    {
        GradientManager.SetGradientEnabled(Player, false);
        Player.cosmetics.SetColor(15);
        Player.cosmetics.gameObject.SetActive(false);
        
        Coroutines.Start(HackingManager.HackEffect());   
        foreach (var node in HackingManager.Instance.nodes)
        {
            HackingManager.ToggleNode(node.id, true);
        }
    }

    public override void OnDeactivate()
    {
        GradientManager.SetGradientEnabled(Player, true);
        Player.cosmetics.SetColor((byte)Player.Data.DefaultOutfit.ColorId);
        Player.cosmetics.gameObject.SetActive(true);
        Player.SetName(Player.Data.PlayerName);
        
        Coroutines.Stop(HackingManager.HackEffect());
        foreach (var node in HackingManager.Instance.nodes)
        {
            HackingManager.ToggleNode(node.id, false);
        }
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }
}