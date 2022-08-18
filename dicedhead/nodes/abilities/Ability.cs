using Godot;
using System;

public abstract class Ability : KinematicBody2D, ICastableAbility
{
    [Signal]
    public delegate void AbilityEnded();


    public AbilityData AbilityData {get; private set;}
    public IAbilityScalingFactors Scaling {get; private set;}


    public abstract Hitbox Hitbox {get;}

    protected Team TargetTeam;

    public override void _Ready()
    {
        Hitbox.Connect(nameof(Hitbox.DealtDamage), this, nameof(OnDamageDealt));
    }

    public void SetupData(AbilityData data, IAbilityScalingFactors scaling)
    {
        AbilityData = data;
        Scaling = scaling;
    }

    public virtual void Cast(Node2D source = null, Vector2? position = null, Vector2? direction = null, Team targetTeam = Team.None)
    {
        if (source != null)
            source.AddChild(this);
        if (position != null)
            GlobalPosition = (Vector2)position;
        TargetTeam = targetTeam;
        Visible = true;
    }

    protected abstract void OnDamageDealt();

    protected virtual void MarkEnded()
    {
        Visible = false;
        Hitbox.CollisionMask = 0;
        CollisionMask = 0;
        EmitSignal(nameof(AbilityEnded));
    }

    protected virtual void UpdateTeam()
    {
        bool damagePlayer = TargetTeam.HasFlag(Team.Hero);
        bool damageMonsters = TargetTeam.HasFlag(Team.Monsters);

        Hitbox.CollisionMask = 0;
        Hitbox.SetCollisionMaskBit(1, damagePlayer);
        Hitbox.SetCollisionMaskBit(2, damageMonsters);
    }

}
