using System;
using Godot;

public class AbilityData
{
    public enum ActionType
    {
        Shoot,
        Slash,
    }

    public enum ObjectType
    {
        Dice,
        Sword,
        HommingMissile
    }

    public struct CombatStats
    {
        public static CombatStats One => new CombatStats(1,1,1);
        public static CombatStats Zero => new CombatStats(0,0,0);

        public CombatStats(float count, float rate, float damage)
        {
            Count = count;
            Rate = rate;
            Damage = damage;
        }
        public float Count;
        public float Rate;
        public float Damage;

        public static CombatStats operator +(CombatStats a, CombatStats b)
        {
            return new CombatStats(a.Count + b.Count, a.Rate + b.Rate, a.Damage + b.Damage);
        }

        public static CombatStats operator -(CombatStats a, CombatStats b)
        {
            return a + (-b);
        }


        public static CombatStats operator *(CombatStats a, CombatStats b)
        {
            return new CombatStats(a.Count * b.Count, a.Rate * b.Rate, a.Damage * b.Damage);
        }

        public static CombatStats operator *(CombatStats a, float b)
        {
            return new CombatStats(a.Count * b, a.Rate * b, a.Damage * b);
        }

        public static CombatStats operator -(CombatStats a)
        {
            return new CombatStats(-a.Count, -a.Rate, -a.Damage);
        }

        public static CombatStats operator *(float b, CombatStats a)
        {
            return (CombatStats.One * b) * a;
        }

        public static CombatStats operator -(float b, CombatStats a)
        {
            return (CombatStats.One * b) - a;
        }

        public CombatStats Power(float pow)
        {
            return Power(One * pow);
        }

        public CombatStats Power(CombatStats pow)
        {
            return new CombatStats(
                Mathf.Pow(Count, pow.Count),
                Mathf.Pow(Rate, pow.Rate),
                Mathf.Pow(Damage, pow.Damage)
            );
        }
    }


    public string IconName {get; set;}
    public string ResourceName {get; set;}
    public ActionType Action {get; set;}
    public ObjectType ManipulatedObject {get; set;}


    public CombatStats StatsBase {get; set;}
    public CombatStats StatsSpreadSafePercent {get; set;}
    public CombatStats StatsPowerScaling {get; set;}
    public int SimultaneousShoots {get; set;} = 1;
    public float SimultaneousAngleCoverage = 45f;
    public float Speed {get; set;} = 10f;
    public bool CollideWithWorld {get; set;} = false;
    public int BounceLife {get; set;} = 4;
    public bool DieOnHit {get; set;} = false;
    public float Lifetime {get; set;} = 1f;


    public float IntensityValue {get; set;}

    public string Name {get; set;}


    public Texture GetIcon()
    {
        return ResourceLoader.Load<Texture>($"res://dicedhead/sprites/abilities/{IconName}.png");
    }

    public CombatStats GetRealStats(int spread, float intensity)
    {
        var spreadMultiplier = StatsSpreadSafePercent + (1 - StatsSpreadSafePercent) * (1f/spread);
        if (IntensityValue <= 0)
            return (StatsBase * intensity).Power(StatsPowerScaling) * spreadMultiplier;
        else
            return StatsBase * spreadMultiplier;
    }

    public string GetEffectOneLine(int spread, float intensity)
    {
        string result = "";
        result += ActionTypeToVerb(Action);
        var stats = GetRealStats(spread, intensity);
        result += $" {Math.Round(stats.Count * stats.Damage, 1)} ";
        result += $"{ObjectTypeToString(ManipulatedObject).ToLower()}";

        return result;
    }

    public PackedScene GetPackedScene()
    {
        return ResourceLoader.Load<PackedScene>($"res://dicedhead/nodes/abilities/{ResourceName}/{ResourceName}.tscn");
    }

    private string ActionTypeToVerb(ActionType action)
    {
        if (Action == ActionType.Shoot)
            return "Shoots";
        else if (Action == ActionType.Slash)
            return "Slashes";
        return "??";
    }

    private string ObjectTypeToString(ObjectType obj)
    {
        return obj.ToString();
    }
}