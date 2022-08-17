using Godot;
using System;

public class SwordAbility : Ability
{
    private Sprite _sprite => GetNode<Sprite>("Sprite");
    public override Hitbox Hitbox => GetNode<Hitbox>("Sprite/Hitbox");
    private AnimationPlayer _animationPlayer => GetNode<AnimationPlayer>("AnimationPlayer");
    private AudioStreamPlayer2D _slashAudio => GetNode<AudioStreamPlayer2D>("SlashAudio");
    private AudioStreamPlayer2D _hitAudio => GetNode<AudioStreamPlayer2D>("HitAudio");
    private Timer _timer => GetNode<Timer>("Timer");


    public float Damage { get => Hitbox.Damage; set => Hitbox.Damage = value; }





    public override void _Ready()
    {
        _timer.Connect("timeout", this, nameof(MarkEnded));
    }

    public override void Cast(Vector2 position, Vector2 direction, Team targetTeam)
    {
        base.Cast(position, direction, targetTeam);
        // TODO : MAKE THIS WORK
        // _timer.Start(AbilityData.Lifetime);
        _animationPlayer.Play("slash");
        _slashAudio.Play();
        UpdateTeam();
        LookAt(GlobalPosition + direction);
    }

    protected override void MarkEnded()
    {
        base.MarkEnded();
    }

    protected override void OnDamageDealt()
    {
        _hitAudio.Play();
    }
}
