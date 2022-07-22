using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class SplashEyes : Control
{
    public static readonly List<int>[] _eyeIndexes = new List<int>[6]
    {
        new List<int>(){3},
        new List<int>(){2,4},
        new List<int>(){1,3,4},
        new List<int>(){1,2,4,5},
        new List<int>(){1,2,4,5,3},
        new List<int>(){1,2,4,5,6,7},
    };

    private List<Control> _eyes;
    private AnimationPlayer _fadePlayer => GetNode<AnimationPlayer>("FadePlayer");

    public override void _Ready()
    {
        _eyes = new List<Control>();
        foreach (var child in GetNode("CenterContainer/EyesAlpha/EyesColor").GetChildren())
        {
            if (child is Control c)
            {
                _eyes.Add(c);
            }
        }
    }

    public void Splash(int roll)
    {
        HideAllEyes();
        foreach (int id in _eyeIndexes[roll])
        {
            _eyes[id-1].Visible = true;
        }

        _fadePlayer.Stop();
        _fadePlayer.Play("fade");
    }

    private void HideAllEyes()
    {
        _eyes.ForEach(c => c.Visible = false);
    }
}
