using System;
using Leopotam.EcsLite;
using UnityEngine;

public class RotationSystem : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<TransformComponent> _transformPool;
    private EcsPool<RotationComponent> _rotationPool;
    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.SetRotationStats, SetRotationStats);
        EcsEventBus.Unsubscribe(GameplayEventType.ResetRotation, ResetRotation);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _transformPool = world.GetPool<TransformComponent>();
        _rotationPool = world.GetPool<RotationComponent>();
        _filter = world.Filter<TransformComponent>()
                       .Inc<RotationComponent>()
                       .Exc<PooledObjectTag>()
                       .End();
        EcsEventBus.Subscribe(GameplayEventType.SetRotationStats, SetRotationStats);
        EcsEventBus.Subscribe(GameplayEventType.ResetRotation, ResetRotation);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var transformComp = ref _transformPool.Get(entity);
            ref var rotationComp = ref _rotationPool.Get(entity);
            Vector3 rotationAxis = rotationComp.ClockwiseRotation ? Vector3.back : Vector3.forward;
            rotationAxis *= Mathf.Sign(transformComp.Transform.localScale.x);
            transformComp.Transform.Rotate(rotationAxis, rotationComp.RotationAngle * Time.deltaTime);
        }
    }

    private void ResetRotation(int entity, EventArgs args)
    {
        var transformComp = _transformPool.Get(entity);
        transformComp.Transform.rotation = Quaternion.identity;
    }

    private void SetRotationStats(int entity, EventArgs args)
    {
        var rotateObjectEventArgs = args as SetRotationStatsEventArgs;
        ref var rotationComp = ref _rotationPool.Get(entity);
        rotationComp.ClockwiseRotation = rotateObjectEventArgs.IsClockwise;
        rotationComp.RotationAngle = rotateObjectEventArgs.RotationSpeed;
    }
}
