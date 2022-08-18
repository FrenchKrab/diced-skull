using Godot;
using System;

public class AutoCaster : Node2D
{
    private AutoCastableAbilityData _data;
    private IAbilityScalingFactors _scaling;
    private AutoCastableAbilityData.Stats _finalStats;


    private Timer _timer => GetNode<Timer>("Timer");

    private int _tickCount = 0;

    public Node2D LockOnTarget = null;
    private Team _targetTeam = Team.None;


    public void SetupAbility(AutoCastableAbilityData data, IAbilityScalingFactors scaling)
    {
        _data = data;
        _scaling = scaling;
        _finalStats = data.GetScaledAutoCastableStats(_scaling);
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
        for (int i = 0; i < _finalStats.SimultaneousShots; ++i)
        {
            float angleOffset;
            if (_finalStats.SimultaneousShots == 1)
                angleOffset = 0;
            else
            {
                float angleCoverageRad = Mathf.Deg2Rad(_finalStats.SimultaneousShotsAngleCoverage);
                angleOffset = Mathf.Lerp(-angleCoverageRad/2,+angleCoverageRad/2,1f*i/(_finalStats.SimultaneousShots-1));
            }

            var ability = AbilityPool.Singleton.GetPooledAbility(_data);

            ability.SetupData(_data, _scaling);
            ability.Cast(GlobalPosition, GlobalTransform.x.Rotated(angleOffset), _targetTeam);
        }

        _tickCount++;
    }
}
