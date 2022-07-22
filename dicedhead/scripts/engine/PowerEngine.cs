using System.Collections.Generic;
using Godot;

public static class PowerEngine
{
    public static float GetIntensity(IEnumerable<string> abilities)
    {
        float intensity = 0;
        HashSet<string> alreadySeen = new HashSet<string>();

        foreach (string id in abilities)
        {
            if (alreadySeen.Contains(id))
                continue;
            alreadySeen.Add(id);

            var data = AbilityLoader.GetAbility(id);
            intensity += data.IntensityValue;
        }
        return intensity;
    }

    public static Dictionary<string, int> GetSpread(IEnumerable<string> abilities)
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