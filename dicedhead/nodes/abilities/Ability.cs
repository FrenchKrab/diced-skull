using Godot;
using System;

public abstract class Ability : KinematicBody2D
{
    [Signal]
    public delegate void AbilityEnded();

    [Export]
    public Color AllyColor = new Color(1,1,1);

    [Export]
    public Color MonsterColor = new Color(1,1,1);

    [Export]
    public Color NeutralColor = new Color(1,1,1);


    public AbilityData AbilityData;

    public abstract Hitbox Hitbox {get;}

    public abstract float Damage {get; set;}

    protected Team TargetTeam;

    public override void _Ready()
    {
        Hitbox.Connect(nameof(Hitbox.DealtDamage), this, nameof(OnDamageDealt));
    }


    public virtual void Cast(Vector2 direction, Team targetTeam = Team.None)
    {
        TargetTeam = targetTeam;
        SetCollisionMaskBit(0, AbilityData.CollideWithWorld);
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

    protected void SetTeam(Sprite sprite)
    {
        bool damagePlayer = TargetTeam.HasFlag(Team.Hero) || (TargetTeam == Team.None && AbilityData.IntensityValue > 0);
        bool damageMonsters = TargetTeam.HasFlag(Team.Monsters) || (TargetTeam == Team.None && AbilityData.IntensityValue <= 0);

        Hitbox.CollisionMask = 0;
        Hitbox.SetCollisionMaskBit(1, damagePlayer);
        Hitbox.SetCollisionMaskBit(2, damageMonsters);

        Color c;
        if (!damagePlayer && !damageMonsters)
            c = NeutralColor;
        else if (damageMonsters)
            c = AllyColor;
        else
            c = MonsterColor;
        sprite.Modulate = c;
    }
}
