using Godot;
using System;

public class NextUnlocks : CanvasLayer
{

    private Control _unlockedHolder => GetNode<Control>("VBox/Unlocked/UnlockedHolder");
    private Label _nextUnlockLabel => GetNode<Label>("VBox/Next/Label2");
    private AudioStreamPlayer _audio => GetNode<AudioStreamPlayer>("Audio");
    private Timer _hideTimer => GetNode<Timer>("Timer");


    public override void _Ready()
    {
        Connect("visibility_changed", this, nameof(OnVisibilityChanged));
        HeroState.Singleton.Connect(nameof(HeroState.PassedLevel), this, nameof(DisplayUnlocks));
        _hideTimer.Connect("timeout", this, nameof(OnHideTimeout));
    }

    private void OnHideTimeout()
    {
        Visible = false;
    }

    private void OnVisibilityChanged()
    {
        if (!Visible)
            return;

    }

    public void DisplayUnlocks()
    {
        Visible = true;
        int level = HeroState.Singleton.UnlockLevel;
        _audio.Play();

        foreach (Node child in _unlockedHolder.GetChildren())
            child.QueueFree();

        var unlocks = AbilityLoader.GetUnlockOrder()[level];
        foreach (var id in unlocks)
        {
            var item = ResourceLoader.Load<PackedScene>("res://dicedhead/ui/nodes/NextUnlocks/UnlockedItem.tscn")
                .Instance<UnlockedItem>();
            item.LoadAbility(id);
            _unlockedHolder.AddChild(item);
        }

        _nextUnlockLabel.Text = AbilityLoader.GetUnlockKillRequired()[level + 1].ToString();

        _hideTimer.Start();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//
//  }
}
