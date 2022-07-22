using Godot;
using System;

public class AbilityButton : Control
{
    [Signal]
    public delegate void AbilityPressed(string id);

    public TextureButton Button => GetNode<TextureButton>("Button");
    public string AbilityId {get; private set;}

    private TextureRect _abilityTexture => GetNode<TextureRect>("VBox/TextureRect");
    private TextureRect _backgroundTexture => GetNode<TextureRect>("Background");
    private Label _abilityLabel => GetNode<Label>("VBox/Label");

    public override void _Ready()
    {
        Button.Connect("pressed", this, nameof(OnButtonPressed));
    }

    public void SetAbility(string name)
    {
        AbilityId = name;
        var data = AbilityLoader.GetAbility(name);
        _abilityTexture.Texture = data.GetIcon();
        _abilityLabel.Text = data.Name;

        if (data.IntensityValue > 0)
        {
            _backgroundTexture.FlipV = true;
            _backgroundTexture.Modulate = new Color(1.0f,0.2f,0.2f);
        }
    }

    private void OnButtonPressed()
    {
        EmitSignal(nameof(AbilityPressed), AbilityId);
    }

}
