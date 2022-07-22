using Godot;
using System;

public class AbilityCaster : Node2D
{
    private AbilityData _data;
    private AbilityData.CombatStats _finalStats;


    private Timer _timer => GetNode<Timer>("Timer");

    private int _tickCount = 0;

    public Node2D LockOnTarget = null;
    private Team _targetTeam = Team.None;


    public void SetupAbility(AbilityData data, AbilityData.CombatStats finalStats)
    {
        _data = data;
        _finalStats = finalStats;
    }

    private void LockOnTargetIfNecessary()
    {
        if (LockOnTarget != null)
            LookAt(LockOnTarget.GlobalPosition);
    }

    public void Cast(Team targetTeam = Team.None)
    {
        _targetTeam = targetTeam;
        _timer.Connect("timeout", this, nameof(AttackTick));
        _timer.WaitTime = 1f/_finalStats.Rate;
        _timer.Start();
        AttackTick();
    }

    public void AttackTick()
    {
        if (_tickCount >= _finalStats.Count)
        {
            GD.Print($"{_tickCount} > {_finalStats.Count}");
            _timer.Stop();
            QueueFree();
            return;
        }

        LockOnTargetIfNecessary();
        for (int i = 0; i < _data.SimultaneousShoots; ++i)
        {
            float angleOffset;
            if (_data.SimultaneousShoots == 1)
                angleOffset = 0;
            else
            {
                float angleCoverageRad = Mathf.Deg2Rad(_data.SimultaneousAngleCoverage);
                angleOffset = Mathf.Lerp(-angleCoverageRad/2,+angleCoverageRad/2,1f*i/(_data.SimultaneousShoots-1));
            }

            var ability = AbilityPool.Singleton.GetAbility(_data);

            ability.GlobalPosition = GlobalPosition;
            ability.Damage = _finalStats.Damage;

            ability.Cast(GlobalTransform.x.Rotated(angleOffset), _targetTeam);
        }

        _tickCount++;
    }
}
