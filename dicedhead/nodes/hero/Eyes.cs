using Godot;
using System;
using System.Collections.Generic;

public class Eyes : Node2D
{
    public List<Node2D> _eyes;

    public override void _Ready()
    {
        LoadEyes();
    }

    public void OpenEyes(int count)
    {
        for (int i = 0; i < _eyes.Count; ++i)
        {
            _eyes[i].Visible = i < count;
        }
    }


    private void LoadEyes()
    {
        _eyes =  new List<Node2D>();
        foreach (var child in GetChildren())
        {
            if (child is Node2D child2d)
                _eyes.Add(child2d);
        }
    }

}
