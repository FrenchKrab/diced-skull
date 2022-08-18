using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GenericProjectileData : AutoCastableAbilityData
{
    public new class Stats : AutoCastableAbilityData.Stats
    {
        public static IEnumerable<string> Fields => new string[]{
            FieldCount, FieldRate, FieldSimultaneousShots, FieldSimultaneousShotsAngleCoverage, FieldDamage
        };

        protected const string FieldDamage = "damage";

        public Stats(ArithmeticDictionary ad) : base(ad)
        {}

        public float Damage => Data[FieldDamage];

        public new static ArithmeticDictionary DictOne => new ArithmeticDictionary(
            Fields, 1f
        );

        public new static ArithmeticDictionary DictZero => DictOne.Zero;
    }

    private const string AutoCasterPath = "res://dicedhead/nodes/abilities/AutoCaster.tscn";

    public float InitialSpeed => Get<float>("speed", 10f);
    public float TerminalSpeed => Get<float>("speed_terminal", InitialSpeed);
    public int BounceCount => Get<int>("bounces", 0);
    public int HitCount => Get<int>("hits", 1);
    public float Lifetime => Get<float>("lifetime", 1f);

    public override PackedScene CastablePacked => ResourceLoader.Load<PackedScene>(AutoCasterPath);

    public new readonly Stats BaseStats;
    public new readonly Stats StatsIntensityGrowth;
    public new readonly Stats StatsIntensityPowerBonus;
    public new readonly Stats StatsSpreadSafePercent;


    public GenericProjectileData(IDictionary data, string id) : base(data, id)
    {
    }


    public override float GetEstimatedAttack(IAbilityScalingFactors scaling)
    {
        var realStats = GetScaledProjectileStats(scaling);
        return realStats.Damage * realStats.Rate * realStats.Count;
    }


    public Stats GetScaledProjectileStats(IAbilityScalingFactors scaling)
    {
        return new Stats(GetScaledStatsDict(scaling.GetSpread(Id), scaling.Intensity));
    }


}