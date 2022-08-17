using Godot;
using System;

public class ProjectileAbility : Ability
{
    private Sprite _sprite => GetNode<Sprite>("Sprite");
    public override Hitbox Hitbox => GetNode<Hitbox>("Area2D");
    private Timer _lifeTimer => GetNode<Timer>("Timer");
    private CollisionShape2D _collider => GetNode<CollisionShape2D>("Collision");
    private AudioStreamPlayer2D _fireAudio => GetNode<AudioStreamPlayer2D>("FireAudio");
    private AudioStreamPlayer2D _bounceAudio => GetNode<AudioStreamPlayer2D>("BounceAudio");
    private AudioStreamPlayer2D _destroyedAudio => GetNode<AudioStreamPlayer2D>("DestroyedAudio");

    public GenericProjectileData ProjectileData => (GenericProjectileData)AbilityData;
    public GenericProjectileData.Stats ScaledStats => ProjectileData.GetScaledProjectileStats(Scaling);

    public float Damage { get => Hitbox.Damage; set => Hitbox.Damage = value; }

    public Vector2 Velocity;
    public float Speed = 50f;

    private int _bounceCount = 0;
    private int _hitCount = 0;

    public override void _Ready()
    {
        base._Ready();
        _lifeTimer.Connect("timeout", this, nameof(MarkEnded));
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!Visible)
            return;

        var lifetimeLerp = (ProjectileData.Lifetime - _lifeTimer.TimeLeft)/ProjectileData.Lifetime;
        Speed = Mathf.Lerp(ProjectileData.InitialSpeed, ProjectileData.TerminalSpeed, lifetimeLerp);
        Velocity = Velocity.Normalized() * Speed;

        var col = MoveAndCollide(Velocity * delta);
        if (col != null)
        {
            Velocity = Velocity.Bounce(col.Normal);
            _bounceCount++;
            if (_bounceCount <= ProjectileData.BounceCount && ProjectileData.BounceCount >= 0)
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
        if (_hitCount > ProjectileData.HitCount)
        {
            CallDeferred(nameof(MarkEnded));
        }
    }


    protected override void UpdateTeam()
    {
        base.UpdateTeam();

        bool damagePlayer = TargetTeam.HasFlag(Team.Hero);
        bool damageMonsters = TargetTeam.HasFlag(Team.Monsters);
        // Color c;
        // if (!damagePlayer && !damageMonsters)
        //     c = NeutralColor;
        // else if (damageMonsters)
        //     c = AllyColor;
        // else
        //     c = MonsterColor;
        // sprite.Modulate = c;
    }

    protected override void MarkEnded()
    {
        Velocity = Vector2.Zero;
        _collider.Disabled = true;
        Visible = false;
        base.MarkEnded();
    }


    public override void Cast(Vector2 position, Vector2 direction, Team targetTeam)
    {
        base.Cast(position, direction, targetTeam);
        ResetState();

        Damage = ScaledStats.Damage;
        Speed = ProjectileData.InitialSpeed;
        Velocity = direction.Normalized() * Speed;
        _fireAudio.Play();
        if (ProjectileData.Lifetime > 0)
        {
            _lifeTimer.Start(ProjectileData.Lifetime);
        }
    }

    private void ResetState()
    {
        Visible = true;
        SetCollisionMaskBit(0, ProjectileData.BounceCount > 0);
        UpdateTeam();
        _collider.Disabled = false;
        GetNode<Particles2D>("Particles2D").Restart();
        _bounceCount = 0;
        _hitCount = 0;
    }
}
