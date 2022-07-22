using Godot;
using System;
using System.Collections.Generic;

public class AbilityPool : Node
{
    public static AbilityPool Singleton {get; private set;}

    private Dictionary<string, List<Ability>> _freePools = new Dictionary<string, List<Ability>>();
    private Dictionary<string, List<Ability>> _busyPools = new Dictionary<string, List<Ability>>();
    private Node2D _poolNode;

    public override void _Ready()
    {
        if (Singleton != null)
            Free();
        Singleton = this;

        GD.Print("test");
        _poolNode = GetNode<Node2D>("Pool");
        foreach (string id in AbilityLoader.GetIds())
        {
            var data = AbilityLoader.GetAbility(id);
            _freePools[data.ResourceName] = new List<Ability>();
            _busyPools[data.ResourceName] = new List<Ability>();
        }
    }

    public Ability GetAbility(AbilityData data)
    {
        var key = data.ResourceName;

        if (_freePools[key].Count > 0)
        {
            var existingAbility = _freePools[key][0];
            _freePools[key].RemoveAt(0);
            _busyPools[key].Add(existingAbility);
            existingAbility.AbilityData = data;
            return existingAbility;
        }
        else
        {
            // var data = AbilityLoader.GetAbility(id);
            var packed = data.GetPackedScene();
            var ability = packed.Instance<Ability>();
            ability.AbilityData = data;
            _busyPools[key].Add(ability);
            ability.Connect(nameof(Ability.AbilityEnded), this, nameof(OnAbilityEnded), new Godot.Collections.Array() {ability});
            _poolNode.AddChild(ability);
            return ability;
        }
    }

    public Ability GetAbility(string id)
    {
        return GetAbility(AbilityLoader.GetAbility(id));
    }

    private void OnAbilityEnded(Ability ability)
    {
        _busyPools[ability.AbilityData.ResourceName].Remove(ability);
        _freePools[ability.AbilityData.ResourceName].Add(ability);
    }
}
