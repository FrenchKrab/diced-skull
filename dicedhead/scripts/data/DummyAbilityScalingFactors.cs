
public class DummyAbilityScalingFactors : IAbilityScalingFactors
{
    public DummyAbilityScalingFactors(float intensity, int spread = 1)
    {
        Intensity = intensity;
        _spread = spread;
    }

    public float Intensity {get; private set;}
    private readonly int _spread;

    public int GetSpread(string id) => _spread;
}