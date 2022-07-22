using Godot;

public class InputUtils : Node
{
    public static InputUtils Singleton {get; private set;}

    public override void _Ready()
    {
        if (Singleton != null)
            Free();
        Singleton = this;
    }

    public static float ActionsToAxis(string actionNeg, string actionPos)
    {
        float value = 0.0f;
        value += Input.GetActionStrength(actionPos);
        value -= Input.GetActionStrength(actionNeg);
        return value;
    }


    public static void LeftClick()
    {
        var clickEvent = new InputEventMouseButton()
        {
            ButtonIndex = 1,
            Pressed = true,
            GlobalPosition = Singleton.GetViewport().GetMousePosition()
        };
        Singleton.GetTree().InputEvent(clickEvent);

        var clickEvent2 = new InputEventMouseButton()
        {
            ButtonIndex = 1,
            Pressed = false,
            GlobalPosition = Singleton.GetViewport().GetMousePosition()
        };
        Singleton.GetTree().InputEvent(clickEvent2);
    }
}