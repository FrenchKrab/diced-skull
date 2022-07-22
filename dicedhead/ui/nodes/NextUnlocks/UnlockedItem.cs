using Godot;
using System;

public class UnlockedItem : HBoxContainer
{
    private Label _label => GetNode<Label>("Label2");
    private TextureRect _textureRect => GetNode<TextureRect>("TextureRect");

    public override void _Ready()
    {

    }

    public void LoadAbility(string id)
    {
        var data = AbilityLoader.GetAbility(id);
        _textureRect.Texture = data.GetIcon();
        _label.Text = data.Name;
    }
}
