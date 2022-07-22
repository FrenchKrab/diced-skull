using Godot;
using System;
using System.Collections.Generic;

public class DiceAbilityHolder : Control
{

    private TextureRect _diceTexture => GetNode<TextureRect>("Dice");
    private Label _effectLabel => GetNode<Label>("VBox/Effect");
    private TextureRect _abilityTexture => GetNode<TextureRect>("VBox/Ability");

    public string AbilityId {get; private set;}
    private AbilityData _abilityData = null;

    public override void _Ready()
    {
        // SetAbility("ranged_dice1");
    }

    public void SetDiceNumber(int number)
    {
        string path = $"res://dicedhead/sprites/misc/dice/dice{number}.png";
        _diceTexture.Texture = ResourceLoader.Load<Texture>(path);
    }

    public void SetAbility(string name)
    {
        var data = AbilityLoader.GetAbility(name);
        AbilityId = name;
        _abilityData = data;
        _abilityTexture.Texture = data.GetIcon();
        _effectLabel.Text = "";
    }


    public void UpdateAbilityText(int spread, float intensity)
    {
        _effectLabel.Text = _abilityData.GetEffectOneLine(spread, intensity);
    }

    public void ClearAbility()
    {
        _abilityTexture.Texture = null;
        _effectLabel.Text = "";
        AbilityId = null;
        _abilityData = null;
    }
}
