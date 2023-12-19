using System;
using System.Diagnostics;
using Leopotam.EcsLite;
using UnityEngine;

public class DirectionSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<MovementStatsComponent> _movementStats;
    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.ChangeDirectionPattern, ChangeDirectionPattern);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<MovementStatsComponent>()
                       .Exc<PooledObjectTag>()
                       .End();
        _movementStats = world.GetPool<MovementStatsComponent>();
        EcsEventBus.Subscribe(GameplayEventType.ChangeDirectionPattern, ChangeDirectionPattern);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var movementStats = ref _movementStats.Get(entity);
            movementStats.MovementDirection = movementStats.MovementDirectionPattern.Value.GetDirection(entity);
        }
    }

    private void ChangeDirectionPattern(int senderEntity, EventArgs args)
    {
        var changeMovementDirectionArgs = args as ChangeMovementDirectionPatternEventArgs;
        ref var movementStats = ref _movementStats.Get(senderEntity);
        movementStats.MovementDirectionPattern = changeMovementDirectionArgs.NewMovementDirectionPattern;
    }
}
