using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public static class AbilityLoader
{
    private static Dictionary<string, AbilityData> _abilities = null;

    private static void SetupAbilitiesIfNeeded()
    {
        if (_abilities != null)
            return;

        _abilities = new Dictionary<string, AbilityData>();

        _abilities["bonus_dice1"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Shoot,
            ManipulatedObject = AbilityData.ObjectType.Dice,
            StatsBase = new AbilityData.CombatStats(1,1,3),
            StatsSpreadSafePercent = new AbilityData.CombatStats(1,1,0),
            StatsPowerScaling = AbilityData.CombatStats.One * 0.3f,
            IconName = "dice2",
            Name = "Die I",
            IntensityValue = 0,
            ResourceName = "dice",
            BounceLife = 1,
            CollideWithWorld = true,
            Lifetime = 2f,
            Speed = 600f,
            DieOnHit = true,
        };

        _abilities["bonus_dice2"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Shoot,
            ManipulatedObject = AbilityData.ObjectType.Dice,
            StatsBase = new AbilityData.CombatStats(10,1,10),
            StatsSpreadSafePercent = new AbilityData.CombatStats(1,1,0),
            StatsPowerScaling = AbilityData.CombatStats.One * 0.6f,
            IconName = "dice2",
            Name = "Die II",
            IntensityValue = 0,
            ResourceName = "dice",
            BounceLife = 2,
            CollideWithWorld = true,
            Lifetime = 3f,
            Speed = 700f,
            DieOnHit = true,
        };

        _abilities["bonus_sdice1"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Shoot,
            ManipulatedObject = AbilityData.ObjectType.Dice,
            StatsBase = new AbilityData.CombatStats(3,3,6),
            StatsSpreadSafePercent = new AbilityData.CombatStats(1,1,0),
            StatsPowerScaling = AbilityData.CombatStats.One * 0.3f,
            SimultaneousShoots = 2,
            SimultaneousAngleCoverage = 25f,
            IconName = "dice2",
            Name = "Die>",
            IntensityValue = 0,
            ResourceName = "dice",
            BounceLife = 3,
            CollideWithWorld = true,
            Lifetime = 4f,
            Speed = 400f,
            DieOnHit = true,
        };

        _abilities["bonus_sdice2"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Shoot,
            ManipulatedObject = AbilityData.ObjectType.Dice,
            StatsBase = new AbilityData.CombatStats(4,4,3),
            StatsSpreadSafePercent = new AbilityData.CombatStats(1,1,0),
            StatsPowerScaling = AbilityData.CombatStats.One * 0.6f,
            SimultaneousShoots = 3,
            SimultaneousAngleCoverage = 45f,
            IconName = "dice2",
            Name = "<Die>",
            IntensityValue = 0,
            ResourceName = "dice",
            BounceLife = 2,
            CollideWithWorld = true,
            Lifetime = 3f,
            Speed = 500f,
            DieOnHit = true,
        };

        _abilities["bonus_bdice1"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Shoot,
            ManipulatedObject = AbilityData.ObjectType.Dice,
            StatsBase = new AbilityData.CombatStats(2,1.5f,1),
            StatsSpreadSafePercent = new AbilityData.CombatStats(1,1,0),
            StatsPowerScaling = AbilityData.CombatStats.One * 0.7f,
            SimultaneousShoots = 1,
            SimultaneousAngleCoverage = 45f,
            IconName = "dice2",
            Name = "B-Dice",
            IntensityValue = 0,
            ResourceName = "dice",
            BounceLife = 20,
            CollideWithWorld = true,
            Lifetime = 6f,
            Speed = 400f,
            DieOnHit = false,
        };

        _abilities["bonus_bdice2"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Shoot,
            ManipulatedObject = AbilityData.ObjectType.Dice,
            StatsBase = new AbilityData.CombatStats(3,1.8f,3),
            StatsSpreadSafePercent = new AbilityData.CombatStats(1,1,0),
            StatsPowerScaling = AbilityData.CombatStats.One * 1.2f,
            SimultaneousShoots = 1,
            SimultaneousAngleCoverage = 45f,
            IconName = "dice2",
            Name = "B-Dice",
            IntensityValue = 0,
            ResourceName = "dice",
            BounceLife = 20,
            CollideWithWorld = true,
            Lifetime = 6f,
            Speed = 600f,
            DieOnHit = false,
        };

        _abilities["bonus_sword1"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Slash,
            ManipulatedObject = AbilityData.ObjectType.Sword,
            StatsBase = new AbilityData.CombatStats(1,1,3),
            StatsSpreadSafePercent = new AbilityData.CombatStats(1,1,0),
            StatsPowerScaling = AbilityData.CombatStats.One * 0.3f,
            IconName = "sword2",
            Name = "Sword I",
            IntensityValue = 0,
            ResourceName = "sword",
            Lifetime = 0.5f,
            BounceLife = 4,
        };

        _abilities["bonus_scissors1"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Slash,
            ManipulatedObject = AbilityData.ObjectType.Sword,
            StatsBase = new AbilityData.CombatStats(1,1,6),
            StatsSpreadSafePercent = new AbilityData.CombatStats(1,1,0),
            StatsPowerScaling = AbilityData.CombatStats.One * 0.3f,
            SimultaneousShoots = 2,
            SimultaneousAngleCoverage = 50f,
            IconName = "sword2",
            Name = "Scissors",
            IntensityValue = 0,
            ResourceName = "sword",
            Lifetime = 0.5f,
            BounceLife = 4,
        };

        _abilities["bonus_scissors2"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Slash,
            ManipulatedObject = AbilityData.ObjectType.Sword,
            StatsBase = new AbilityData.CombatStats(1,1,6),
            StatsSpreadSafePercent = new AbilityData.CombatStats(1,1,0),
            StatsPowerScaling = AbilityData.CombatStats.One * 1.1f,
            SimultaneousShoots = 2,
            SimultaneousAngleCoverage = 50f,
            IconName = "sword2",
            Name = "Scissors",
            IntensityValue = 0,
            ResourceName = "sword",
            Lifetime = 0.7f,
            BounceLife = 4,
        };

        _abilities["malus_dice1"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Shoot,
            ManipulatedObject = AbilityData.ObjectType.Dice,
            StatsBase = new AbilityData.CombatStats(3,1f,1),
            StatsSpreadSafePercent = new AbilityData.CombatStats(0,0,0.5f),
            // StatsPowerScaling = new AbilityData.CombatStats(0.3f, 0.3f, 0.3f),
            IconName = "dice3",
            Name = "I Die",
            IntensityValue = 1,
            Lifetime = 5f,
            CollideWithWorld = false,
            ResourceName = "dice",
            Speed = 250f,
            DieOnHit = true,
        };

        _abilities["malus_dice2"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Shoot,
            ManipulatedObject = AbilityData.ObjectType.Dice,
            StatsBase = new AbilityData.CombatStats(12,3f,1),
            StatsSpreadSafePercent = new AbilityData.CombatStats(0,0,1),
            // StatsPowerScaling = new AbilityData.CombatStats(0.3f, 0.3f, 0.3f),
            SimultaneousShoots = 1,
            SimultaneousAngleCoverage = 45f,
            IconName = "dice3",
            Name = "II Die",
            IntensityValue = 5,
            Lifetime = 5f,
            CollideWithWorld = false,
            ResourceName = "dice",
            Speed = 250f,
            DieOnHit = true,
        };

        _abilities["malus_dice3"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Shoot,
            ManipulatedObject = AbilityData.ObjectType.Dice,
            StatsBase = new AbilityData.CombatStats(25,5f,1),
            StatsSpreadSafePercent = new AbilityData.CombatStats(0,0,1),
            // StatsPowerScaling = new AbilityData.CombatStats(0.3f, 0.3f, 0.3f),
            SimultaneousShoots = 1,
            SimultaneousAngleCoverage = 45f,
            IconName = "dice3",
            Name = "II Die",
            IntensityValue = 5,
            Lifetime = 10f,
            CollideWithWorld = false,
            ResourceName = "dice",
            Speed = 250f,
            DieOnHit = true,
        };

        _abilities["malus_sdice1"] = new AbilityData()
        {
            Action = AbilityData.ActionType.Shoot,
            ManipulatedObject = AbilityData.ObjectType.Dice,
            StatsBase = new AbilityData.CombatStats(10,1.5f,1),
            StatsSpreadSafePercent = new AbilityData.CombatStats(0,0,1),
            StatsPowerScaling = new AbilityData.CombatStats(0.3f, 0.3f, 0.3f),
            SimultaneousShoots = 3,
            SimultaneousAngleCoverage = 45f,
            IconName = "dice3",
            Name = ">Die<",
            IntensityValue = 10,
            Lifetime = 5f,
            CollideWithWorld = false,
            ResourceName = "dice",
            Speed = 200f,
            DieOnHit = true,
        };
    }

    public static List<string>[] GetUnlockOrder()
    {
        return new List<string>[] {
            new List<string>() {"bonus_dice1", "malus_dice1"},
            new List<string>() {"bonus_bdice1", "malus_dice2"},
            new List<string>() {"bonus_sword1", "malus_sdice1"},
            new List<string>() {"bonus_scissors1", "malus_dice3"},
        };
    }

    public static int[] GetUnlockKillRequired()
    {
        return new int[]
        {
            0,
            1,
            2,
            3,
            999
        };
    }

    public static List<string> GetIds()
    {
        SetupAbilitiesIfNeeded();

        return _abilities.Keys.ToList();
    }

    public static AbilityData GetAbility(string id)
    {
        SetupAbilitiesIfNeeded();

        return _abilities[id];
    }
}