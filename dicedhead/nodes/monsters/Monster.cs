using Godot;
using System;

public class Monster : KinematicBody2D
{
    [Export]
    public float Life = 5f;

    [Export]
    public float Speed = 150f;

    [Export]
    public float OptimalDistance = 200f;

    [Export]
    public float Intensity = 1f;

    [Export]
    public float CastDelay = 1.5f;

    [Export]
    public bool RandomAbilityOrder = false;

    [Export]
    public string[] Abilities;

    [Export]
    public int KillWorth = 1;


    private PackedScene _abilityCasterPacked;
    private Area2D _detectionArea => GetNode<Area2D>("DetectionArea");
    private Timer _castTimer => GetNode<Timer>("CastTimer");
    private Node2D _cursor => GetNode<Node2D>("Cursor");
    private Hurtbox _hurtbox => GetNode<Hurtbox>("Hurtbox");
    private Node2D _shadow => GetNode<Node2D>("Shadow");

    private Node2D _target = null;

    private int _lastCastAbility = -1;
    private float _initialShadowSize;
    private float _initialLife;
    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        _initialShadowSize = _shadow.Scale.x;
        _initialLife = Life;
        _hurtbox.Connect(nameof(Hurtbox.TookDamage), this, nameof(OnTookDamage));
        _abilityCasterPacked = ResourceLoader.Load<PackedScene>("res://dicedhead/nodes/abilities/AbilityCaster.tscn");
        _detectionArea.Connect("body_entered", this, nameof(OnBodyEnterDetectionRadius));
        _castTimer.Connect("timeout", this, nameof(OnCastPossible));
        _castTimer.Start();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_target != null)
        {
            var offset = (_target.GlobalPosition - GlobalPosition);
            var direction = offset.Normalized();
            if (offset.Length() < OptimalDistance)
                direction = -direction;

            MoveAndSlide(direction * Speed);
        }
    }

    private void OnTookDamage(float damages)
    {
        GD.Print("Monster took damages !");
        ApplyDamage(damages);
    }

    private void OnCastPossible()
    {
        if (_target == null || Abilities.Length == 0)
            return;

        int abilityNumber;
        if (RandomAbilityOrder)
        {
            abilityNumber = _rng.RandiRange(0,Abilities.Length-1);
        }
        else
        {
            _lastCastAbility = (_lastCastAbility+1)%Abilities.Length;
            abilityNumber = _lastCastAbility;
        }

        var caster = _abilityCasterPacked.Instance<AutoCaster>();
        var data = AbilityLoader.GetAbility(Abilities[abilityNumber]);
        AddChild(caster);
        caster.SetupAbility(data, new DummyAbilityScalingFactors(Intensity));
        caster.LockOnTarget = _target;
        caster.Cast(Team.Hero);
    }

    private void OnBodyEnterDetectionRadius(Node body)
    {
        if (body is Hero hero)
        {
            _target = hero;
        }
    }

    public void ApplyDamage(float damages)
    {
        Life -= damages;

        _shadow.Scale = new Vector2(_initialShadowSize * Mathf.Lerp(0f, 1f, Life/_initialLife), _shadow.Scale.y);

        if (Life <= 0)
        {
            HeroState.Singleton.AddKill(KillWorth);

            var deadBody = ResourceLoader.Load<PackedScene>("res://dicedhead/nodes/monsters/DeadBody.tscn").Instance<Node2D>();
            deadBody.GlobalPosition = GlobalPosition;
            GetParent().AddChild(deadBody);
            QueueFree();
        }
    }
}
