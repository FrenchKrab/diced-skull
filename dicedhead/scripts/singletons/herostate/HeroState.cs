using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class HeroState : Node
{
    [Signal]
    public delegate void Killed();

    [Signal]
    public delegate void PassedLevel();

    public static HeroState Singleton {get; private set;}

    public int Kills {get; private set;} = 0;
    public int UnlockLevel {get; private set;} = 0;

    private HashSet<string> _unlockedAbilities = new HashSet<string>();

    private Dictionary<string, AbilityData.CombatStats> _cachedFinalStats;
    public string[] _diceAbilities;

    public override void _Ready()
    {
        if (Singleton != null)
            Free();
        Singleton = this;

        UnlockAbilitiesAtLevel(0);
        // for (int i = 0; i < 4; ++i)
        //     UnlockAbilitiesAtLevel(i);

        ChooseAbilities(new string[6] {"bonus_dice1","bonus_dice1","bonus_dice1","malus_dice1","malus_dice1","malus_dice1"});
    }

    public void ChooseAbilities(IEnumerable<string> abilities)
    {
        GD.Print($"abilities = {abilities.Count()}");

        var intensity = PowerEngine.GetIntensity(abilities);
        var spread = PowerEngine.GetSpread(abilities);

        _cachedFinalStats = new Dictionary<string, AbilityData.CombatStats>();
        foreach(var id in abilities)
        {
            var data = AbilityLoader.GetAbility(id);
            _cachedFinalStats[id] = data.GetRealStats(spread[id], intensity);
        }

        _diceAbilities = abilities.ToArray();
    }

    public string GetDiceAbility(int rollNumber)
    {
        return _diceAbilities[rollNumber];
    }

    public AbilityData.CombatStats GetFinalStats(int rollNumber)
    {
        return _cachedFinalStats[GetDiceAbility(rollNumber)];
    }


    public void UnlockAbilitiesAtLevel(int level)
    {
        var newUnlocks = AbilityLoader.GetUnlockOrder()[level];
        foreach (var unlock in newUnlocks)
            UnlockAbility(unlock);
    }

    private void UnlockAbility(string id)
    {
        _unlockedAbilities.Add(id);
    }

    public IEnumerable<string> GetUnlockedAbilities()
    {
        return _unlockedAbilities;
    }

    public void AddKill(int count = 1)
    {
        for (int i = 0; i < count; ++i)
        {
            Kills++;
            if (Kills >= AbilityLoader.GetUnlockKillRequired()[UnlockLevel+1])
            {
                UnlockLevel++;
                UnlockAbilitiesAtLevel(UnlockLevel);
                EmitSignal(nameof(PassedLevel));
            }

            EmitSignal(nameof(Killed));
        }
    }
}
