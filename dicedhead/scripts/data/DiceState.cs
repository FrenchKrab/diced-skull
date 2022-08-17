using System;
using System.Collections.Generic;
using System.Linq;

public class DiceState : IAbilityScalingFactors
{
    public DiceState(int sides)
    {
        _abilities = new string[sides];
        _abilityDataCache = new AbilityData[sides];
    }

    public int Sides => _abilities.Length;
    public string[] Abilities => (string[])_abilities.Clone();
    public float Intensity {get; private set;}

    private readonly string[] _abilities;
    private readonly AbilityData[] _abilityDataCache;
    private readonly Dictionary<string, int> _abilitySpreadCache;


    public void ChooseAbilities(IEnumerable<string> newAbilities)
    {
        string[] newArr = newAbilities.ToArray();
        Array.Copy(newArr, _abilities, Math.Min(newArr.Length, Sides));
        for (int i = newArr.Length; i < Sides; ++i)
            _abilities[i] = null;

        UpdateAbilityDataCache();
        UpdateSpread();
        UpdateIntensity();
    }

    public string GetRollAbilityId(int rollNumber)
    {
        return _abilities[rollNumber];
    }

    public AbilityData GetRollAbilityData(int rollNumber)
    {
        return _abilityDataCache[rollNumber];
    }

    public int GetSpread(string id)
    {
        if (_abilitySpreadCache.TryGetValue(id, out int spread))
            return spread;
        else
            return 0;
    }

    public int GetSpread(int roll)
    {
        return GetSpread(GetRollAbilityId(roll));
    }



    private void UpdateAbilityDataCache()
    {
        for (int i = 0; i < Sides; ++i)
        {
            var id = _abilities[i];
            _abilityDataCache[i] = id == null ? null : AbilityLoader.GetAbility(id);
        }
    }

    private void UpdateIntensity()
    {
        Intensity = 0;
        HashSet<string> alreadySeen = new HashSet<string>();

        for (int i = 0; i < Sides; ++i)
        {
            var id = GetRollAbilityId(i);
            if (alreadySeen.Contains(id))
                continue;
            alreadySeen.Add(id);

            var data = GetRollAbilityData(i);
            Intensity += data.IntensityValue;
        }
    }

    private void UpdateSpread()
    {
        _abilitySpreadCache.Clear();
        _abilitySpreadCache.Union(ComputeSpread(Abilities));
    }


    public static Dictionary<string, int> ComputeSpread(IEnumerable<string> abilities)
    {
        Dictionary<string, int> spread = new Dictionary<string, int>();

        foreach (string id in abilities)
        {
            if (!spread.ContainsKey(id))
            {
                spread[id] = 1;
                continue;
            }
            else
            {
                spread[id] += 1;
            }
        }
        return spread;
    }

}