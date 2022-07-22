using Godot;
using System;

public class Hitbox : Area2D
{
    [Signal]
    public delegate void DealtDamage();

    public float Damage;

    public override void _Ready()
    {
        Connect("area_entered", this, nameof(OnAreaEntered));
        Connect("body_entered", this, nameof(OnBodyEntered));
    }

    private void ApplyDamage(IDamageable damageable)
    {
        damageable.ApplyDamage(Damage);
        EmitSignal(nameof(DealtDamage));
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is IDamageable damageable)
        {
            ApplyDamage(damageable);
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (body is IDamageable damageable)
        {
            ApplyDamage(damageable);
        }
    }
}
