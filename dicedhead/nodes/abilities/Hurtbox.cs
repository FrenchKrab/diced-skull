using Godot;
using System;

public class Hurtbox : Area2D, IDamageable
{
    [Signal]
    public delegate void TookDamage(float damages);


    public void ApplyDamage(float damages)
    {
        EmitSignal(nameof(TookDamage), damages);
    }
}
