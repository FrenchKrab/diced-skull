using Godot;
using System;

public class Goal : Area2D
{
    [Export]
    public string NextLevelName = "floor1";


    public override void _Ready()
    {
        Connect("body_entered", this, nameof(OnBodyEntered));
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Hero hero)
        {
            foreach (Node child in GetTree().Root.GetChildren())
            {
                if (child.Name.Contains("level") || child.Name.Contains("floor"))
                {
                    child.QueueFree();
                }
            }

            var level = ResourceLoader.Load<PackedScene>($"res://dicedhead/nodes/levels/{NextLevelName}.tscn")
                .Instance<Node>();
            GetTree().Root.AddChild(level);

            hero.GlobalPosition = Vector2.Zero;
            hero.DiceComposition.Visible = true;
            hero.Life = Hero.MaxLife;
            hero.ApplyDamage(0);
        }
    }
}
