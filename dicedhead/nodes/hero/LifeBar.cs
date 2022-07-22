using Godot;
using System;

public class LifeBar : CanvasLayer
{
    const int Width = 480;
    const float minDb = -50f;
    const float maxDb = 2f;


    private Control _left => GetNode<Control>("Left");
    private Control _right => GetNode<Control>("Right");
    private AudioStreamPlayer _audio => GetNode<AudioStreamPlayer>("Audio");


    private Vector2 _ogLeftRect, _ogRightRect;


    public override void _Ready()
    {
        _ogLeftRect = _left.RectPosition;
        _ogRightRect = _right.RectPosition;
        UpdateLife(Hero.MaxLife);
        _audio.Play();
    }


    public void UpdateLife(float life)
    {
        var percent = life/Hero.MaxLife;
        _left.RectPosition = _ogLeftRect + Vector2.Left * percent * Width/2;
        _right.RectPosition = _ogRightRect + Vector2.Right * percent * Width/2;
        Color c = new Color(1f,1f,1f,1f-percent);
        _left.Modulate = c;
        _right.Modulate = c;
        _audio.VolumeDb = Mathf.Lerp(minDb, maxDb, (1-percent));
    }

}
