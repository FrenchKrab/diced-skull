using Godot;
using System;

public class test1 : Node
{

    public override void _Ready()
    {
        var a = AbilityData.LoadDictFromInternalFile("test1");
        var data = new GenericProjectileData(a);
        GD.Print(data.Class);
    }

}
