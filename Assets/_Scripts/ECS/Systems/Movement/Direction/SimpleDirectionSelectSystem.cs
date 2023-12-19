using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDirectionSelectSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    EcsFilter _filter;
    EcsPool<MovementStatsComponent> _movementStatsPool;
    //EcsPool<PhysicalBodyComponent> _physicalBodyPool;
    EcsPool<TransformComponent> _transformPool;
    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _movementStatsPool = null;
        _transformPool = null;
        //_physicalBodyPool = null;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<SimpleMovementTag>().Inc<PhysicalBodyComponent>().Exc<PooledObjectTag>().End();
        _movementStatsPool = world.GetPool<MovementStatsComponent>();
       // _physicalBodyPool = world.GetPool<PhysicalBodyComponent>();
        _transformPool = world.GetPool<TransformComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var stats = ref _movementStatsPool.Get(entity);
            //ref var physicalBody = ref _physicalBodyPool.Get(entity);
            ref var transform = ref _transformPool.Get(entity);

            stats.MovementDirection = transform.Transform.right;
            /*
            var targetSpeed = stats.MovementSpeed * new Vector2(directionVector.x, directionVector.y).normalized;
            var runAccelAmount = (50f * stats.Acceleration) / stats.MovementSpeed;
            var runDeccelAmount = (50f * stats.Decceleration) / stats.MovementSpeed;
            float accelRate = Mathf.Abs(targetSpeed.magnitude) > 0.01f ? runAccelAmount : runDeccelAmount;
            var speedDiff = targetSpeed - stats.Movement;
            var movement = speedDiff * accelRate;
            stats.Movement = new Vector2(stats.Movement.x + (Time.fixedDeltaTime * movement.x) / physicalBody.RigidBody.mass,
                                         stats.Movement.y + (Time.fixedDeltaTime * movement.y) / physicalBody.RigidBody.mass);
            */
        }
    }
}
