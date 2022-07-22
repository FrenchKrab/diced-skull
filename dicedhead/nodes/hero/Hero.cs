using Godot;
using System;

public class Hero : KinematicBody2D, IDamageable
{
    public enum GameState
    {
        InGame,
        DiceComposition,
    }


    [Signal]
    public delegate void Rolled(int number, float time);

    [Signal]
    public delegate void RollEnd();

    public const float MaxLife = 10f;


    public GameState CurrentGameState => DiceComposition.Visible ? GameState.DiceComposition : GameState.InGame;

    public float Life {get; set;} = MaxLife;


    private const float MoveSpeed = 300f;

    private PackedScene _abilityCasterPacked;
    private AnimationPlayer _spriteAnimation => GetNode<AnimationPlayer>("sprite/AnimationPlayer");
    private AnimationPlayer _headAnimation => GetNode<AnimationPlayer>("sprite/AnimationPlayerHead");
    private AudioStreamPlayer2D _hitAudio => GetNode<AudioStreamPlayer2D>("HitAudio");
    private AudioStreamPlayer2D _rollAudio => GetNode<AudioStreamPlayer2D>("RollAudio");
    private Eyes _eyes => GetNode<Eyes>("sprite/Head/Eyes");
    public DiceComposition DiceComposition => GetNode<DiceComposition>("CanvasLayer/DiceComposition");
    private LifeBar _lifebar => GetNode<LifeBar>("Lifebar");

    private Node2D _cursor => GetNode<Node2D>("Cursor");
    private Timer _abilityDelayTimer => GetNode<Timer>("AbilityDelay");

    private int _roll = 0;
    private bool _inAbilityDelay = false;

    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        _rng.Randomize();
        _abilityCasterPacked = ResourceLoader.Load<PackedScene>("res://dicedhead/nodes/abilities/AbilityCaster.tscn");
        _abilityDelayTimer.Connect("timeout", this, nameof(EndRollDelay));
    }

    public override void _Process(float delta)
    {
        if (CurrentGameState == GameState.InGame)
        {
            Vector2 aimInputs = new Vector2(
                InputUtils.ActionsToAxis("aim_left", "aim_right"),
                InputUtils.ActionsToAxis("aim_down", "aim_up")
            );

            if (aimInputs.Length() > 0.3f)
            {
                float angle = Mathf.Atan2(-aimInputs.y, aimInputs.x);
                _cursor.Rotation = angle;
            }

            if (Input.IsActionJustPressed("roll"))
            {
                if (_inAbilityDelay)
                {
                    EndRollDelay();
                }
                _rollAudio.Play();
                _headAnimation.Play("RESET");
                _headAnimation.Play("roll");
                _roll = _rng.RandiRange(0, 5);
                _eyes.OpenEyes(_roll+1);

                float delay = 0.01f;
                EmitSignal(nameof(Rolled), _roll, delay);
                GetNode<SplashEyes>("EyesSplash").Splash(_roll);
                _inAbilityDelay = true;
                _abilityDelayTimer.Start(delay);
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if (CurrentGameState == GameState.InGame)
        {
            Vector2 moveInputs = new Vector2(
                InputUtils.ActionsToAxis("ui_left", "ui_right"),
                InputUtils.ActionsToAxis("ui_up", "ui_down")
            );
            moveInputs = moveInputs.Normalized();

            string animationName = "idle";
            if (moveInputs.Length() > 0.5f)
                animationName = "walk";
            if (_spriteAnimation.CurrentAnimation != animationName)
                _spriteAnimation.Play(animationName);

            MoveAndSlide(moveInputs * MoveSpeed);
        }
    }

    public void ApplyDamage(float damages)
    {
        if (CurrentGameState != GameState.InGame)
            return;
        Life -= damages;
        _lifebar.UpdateLife(Life);
        // GD.Print($"new life={Life}");
        _hitAudio.Play();

        if (Life < 0f)
        {
            GetTree().Quit();
        }
    }

    private void EndRollDelay()
    {
        _inAbilityDelay = false;
        _abilityDelayTimer.Stop();
        EmitSignal(nameof(RollEnd));

        var caster = _abilityCasterPacked.Instance<AbilityCaster>();
        var data = AbilityLoader.GetAbility(HeroState.Singleton.GetDiceAbility(_roll));

        if (data.IntensityValue > 0)
        {
            caster.LockOnTarget = this;

            GetTree().Root.AddChild(caster);
            float randAngle = _rng.Randf() * Mathf.Pi*2;
            caster.GlobalPosition = GlobalPosition +  Vector2.One.Rotated(randAngle) * 250f;
            GD.Print(caster.GlobalPosition);
        }
        else
        {
            GetNode<Node2D>("Cursor").AddChild(caster);
        }
        caster.SetupAbility(data, HeroState.Singleton.GetFinalStats(_roll));
        caster.Cast();
    }
}
