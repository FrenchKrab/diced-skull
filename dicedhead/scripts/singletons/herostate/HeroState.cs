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


    public DiceState DiceState {get; private set;} = new DiceState(6);
    public int Kills {get; private set;} = 0;
    public int UnlockLevel {get; private set;} = 0;


    private HashSet<string> _unlockedAbilities = new HashSet<string>();


    public override void _Ready()
    {
        if (Singleton != null)
            Free();
        Singleton = this;

        UnlockAbilitiesAtLevel(0);
        // for (int i = 0; i < 4; ++i)
        //     UnlockAbilitiesAtLevel(i);

        DiceState.ChooseAbilities(new string[6] {"bonus_dice1","bonus_dice1","bonus_dice1","malus_dice1","malus_dice1","malus_dice1"});
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
