using Godot;
using System;

public class WorldMonster : StaticBody2D, IDamageable
{
    [Export]
    public float Life = 10;

    private AnimationPlayer _animation => GetNode<AnimationPlayer>("AnimationPlayer");
    private AudioStreamPlayer2D _dieAudio => GetNode<AudioStreamPlayer2D>("DieAudio");
    private AudioStreamPlayer2D _livingAudio => GetNode<AudioStreamPlayer2D>("LivingAudio");

    public void ApplyDamage(float damages)
    {
        if (Life <= 0)
            return;
        Life -= damages;
        KillIfDead();
    }



    public override void _Ready()
    {

    }

    private void KillIfDead()
    {
        if (Life > 0)
            return;

        CollisionLayer = 0;
        _animation.Play("die");
        _dieAudio.Play();
        _livingAudio.Stop();
    }
}
