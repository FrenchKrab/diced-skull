using Godot;

public interface ICastableAbility
{
    void SetupData(AbilityData data, IAbilityScalingFactors scaling);

    void Cast(Node2D source = null, Vector2? position = null, Vector2? direction = null, Team targetTeam = Team.None);
}