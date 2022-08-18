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

        _poolNode = GetNode<Node2D>("Pool");
        foreach (string id in AbilityLoader.GetIds())
        {
            var data = AbilityLoader.GetAbility(id);
            _freePools[data.PooledResourceName] = new List<Ability>();
            _busyPools[data.PooledResourceName] = new List<Ability>();
        }
    }

    public Ability GetPooledAbility(AbilityData data)
    {
        var key = data.PooledResourceName;

        if (_freePools[key].Count > 0)
        {
            var existingAbility = _freePools[key][0];
            _freePools[key].RemoveAt(0);
            _busyPools[key].Add(existingAbility);
            return existingAbility;
        }
        else
        {
            // var data = AbilityLoader.GetAbility(id);
            var packed = data.PooledResourcePacked;
            var ability = packed.Instance<Ability>();
            _busyPools[key].Add(ability);
            ability.Connect(nameof(Ability.AbilityEnded), this, nameof(OnAbilityEnded), new Godot.Collections.Array() {ability});
            _poolNode.AddChild(ability);
            return ability;
        }
    }

    public Ability GetAbility(string id)
    {
        return GetPooledAbility(AbilityLoader.GetAbility(id));
    }

    private void OnAbilityEnded(Ability ability)
    {
        _busyPools[ability.AbilityData.PooledResourceName].Remove(ability);
        _freePools[ability.AbilityData.PooledResourceName].Add(ability);
    }
}
