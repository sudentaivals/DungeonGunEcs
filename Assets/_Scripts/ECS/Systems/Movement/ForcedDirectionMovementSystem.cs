using System;
using Leopotam.EcsLite;
using UnityEngine;

public class ForcedDirectionMovementSystem : IEcsInitSystem, IEcsDestroySystem
{
    private EcsPool<ForcedMovementDirectionComponent> _specialMovementPool;
    private EcsPool<MovementStatsComponent> _movementStatsPool;
    private EcsPool<TransformComponent> _transformPool;
    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.ActivateSpecialMovement, ActivateMovement);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _specialMovementPool = world.GetPool<ForcedMovementDirectionComponent>();
        _transformPool = world.GetPool<TransformComponent>();
        _movementStatsPool = world.GetPool<MovementStatsComponent>();
        EcsEventBus.Subscribe(GameplayEventType.ActivateSpecialMovement, ActivateMovement);
    }

    private void ActivateMovement(int entity, EventArgs args)
    {
        ref var statsComp = ref _movementStatsPool.Get(entity);
        ref var transformComp = ref _transformPool.Get(entity);
        var direction = statsComp.MovementDirection == Vector2.zero ? new Vector2(1, 0) * transformComp.Transform.localScale.x : statsComp.MovementDirection;
        ref var specialMovement = ref _specialMovementPool.Get(entity);
        
        specialMovement.Direction = direction;
    }
}
