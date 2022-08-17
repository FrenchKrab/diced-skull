using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class AutoCastableAbilityData : AbilityData
{
    public class Stats
    {
        public Stats(ArithmeticDictionary ad)
        {
            Data = ad;
        }

        protected const string FieldRate = "rate";
        protected const string FieldCount = "count";
        protected const string FieldSimultaneousShots = "simultaneous_shots";
        protected const string FieldSimultaneousShotsAngleCoverage = "simultaneous_shots_angle";

        private static IEnumerable<string> Fields => new string[]{
            FieldCount, FieldRate, FieldSimultaneousShots, FieldSimultaneousShotsAngleCoverage
        };

        public IEnumerable<string> RequiredFields => Fields;

        public readonly ArithmeticDictionary Data;
        public ArithmeticDictionary D => Data;

        public float Rate => Data["rate"];
        public float Count => Data["count"];
        public float SimultaneousShots => Data["simultaneous_shots"];
        public float SimultaneousShotsAngleCoverage => Data["simultaneous_shots_angle"];

        public static ArithmeticDictionary DictOne => new ArithmeticDictionary(
            Fields, 1f
        );

        public static ArithmeticDictionary DictZero => DictOne.Zero;
    }



    public AutoCastableAbilityData(IDictionary data, string id) : base(data, id)
    {
        BaseStats = new Stats(GetArithDict("stats_base", Stats.DictOne));
        StatsSpreadSafePercent = new Stats(GetArithDict("stats_spread_safe", Stats.DictOne));
        StatsIntensityGrowth = new Stats(GetArithDict("stats_intensity_growth", Stats.DictZero));
        StatsIntensityPowerBonus = new Stats(GetArithDict("stats_intensity_power", Stats.DictOne));
    }

    public readonly Stats BaseStats;
    public readonly Stats StatsIntensityGrowth;
    public readonly Stats StatsIntensityPowerBonus;
    public readonly Stats StatsSpreadSafePercent;



    protected ArithmeticDictionary GetScaledStatsDict(int spread, float intensity)
    {
        var spreadMultiplier = StatsSpreadSafePercent.D + (1 - StatsSpreadSafePercent.D) * (1f/spread);
        if (IntensityValue <= 0)
            return BaseStats.D + (StatsIntensityGrowth.D * intensity).Power(StatsIntensityPowerBonus.D) * spreadMultiplier;
        else
            return BaseStats.D * spreadMultiplier;
    }

    public Stats GetScaledAutoCastableStats(IAbilityScalingFactors state)
    {
        return new Stats(GetScaledStatsDict(state.GetSpread(Id), state.Intensity));
    }
}