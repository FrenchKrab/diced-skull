using Godot;
using System;

public class DiceAbility : Ability
{
    private Sprite _sprite => GetNode<Sprite>("Sprite");
    public override Hitbox Hitbox => GetNode<Hitbox>("Area2D");
    private Timer _lifeTimer => GetNode<Timer>("Timer");
    private CollisionShape2D _collider => GetNode<CollisionShape2D>("Collision");
    private AudioStreamPlayer2D _fireAudio => GetNode<AudioStreamPlayer2D>("FireAudio");
    private AudioStreamPlayer2D _bounceAudio => GetNode<AudioStreamPlayer2D>("BounceAudio");
    private AudioStreamPlayer2D _destroyedAudio => GetNode<AudioStreamPlayer2D>("DestroyedAudio");

    public override float Damage { get => Hitbox.Damage; set => Hitbox.Damage = value; }

    public Vector2 Velocity;
    public float Speed = 50f;

    private int _bounceCount = 0;

    public override void _Ready()
    {
        base._Ready();
        _lifeTimer.Connect("timeout", this, nameof(MarkEnded));
    }

    public override void _PhysicsProcess(float delta)
    {
        var col = MoveAndCollide(Velocity * delta);
        if (col != null)
        {
            Velocity = Velocity.Bounce(col.Normal);
            _bounceCount++;
            if (_bounceCount <= AbilityData.BounceLife && AbilityData.BounceLife >= 0)
                _bounceAudio.Play();
            else
            {
                MarkEnded();
            }
        }
    }

    protected override void OnDamageDealt()
    {
        _destroyedAudio.Play();
        if (AbilityData.DieOnHit)
        {

            CallDeferred(nameof(MarkEnded));
        }
    }



    protected override void MarkEnded()
    {
        Velocity = Vector2.Zero;
        _collider.Disabled = true;
        base.MarkEnded();
    }


    public override void Cast(Vector2 direction, Team targetTeam)
    {
        base.Cast(direction, targetTeam);
        SetTeam(_sprite);
        _collider.Disabled = false;
        GetNode<Particles2D>("Particles2D").Restart();
        _bounceCount = 0;
        Speed = AbilityData.Speed;
        Velocity = direction.Normalized() * Speed;
        _fireAudio.Play();
        if (AbilityData.Lifetime > 0)
        {
            _lifeTimer.Start(AbilityData.Lifetime);
        }
    }
}
