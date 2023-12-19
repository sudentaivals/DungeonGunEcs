using Leopotam.EcsLite;
using System;
using System.Linq;
using UnityEngine;

public class MovementSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<MovementStatsComponent> _movementStatsPool;
    private EcsPool<PhysicalBodyComponent> _physicalBodyPool;

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.SetMovementStatus, SetMovement);
        EcsEventBus.Unsubscribe(GameplayEventType.RemoveMovement, RemoveMovementVector);
        EcsEventBus.Unsubscribe(GameplayEventType.ChangeMovementSpeed, ChangeMovementSpeed);
    }

    private void SetMovement(int senderEntity, EventArgs args)
    {
        var newStats = args as SetMovementEventArgs;
        ref var stats = ref _movementStatsPool.Get(senderEntity);
        stats.IsMovementAvailable = newStats.NewMovementStatus;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<MovementStatsComponent>()
                       .Inc<PhysicalBodyComponent>()
                       .Exc<PooledObjectTag>()
                       .End();
        _movementStatsPool = world.GetPool<MovementStatsComponent>();
        _physicalBodyPool = world.GetPool<PhysicalBodyComponent>();
        EcsEventBus.Subscribe(GameplayEventType.SetMovementStatus, SetMovement);
        EcsEventBus.Subscribe(GameplayEventType.RemoveMovement, RemoveMovementVector);
        EcsEventBus.Subscribe(GameplayEventType.ChangeMovementSpeed, ChangeMovementSpeed);
    }

    private void RemoveMovementVector(int entity, EventArgs args)
    {
        ref var stats = ref _movementStatsPool.Get(entity);
        stats.Movement = Vector2.zero;
    }

    private void ChangeMovementSpeed(int entity, EventArgs args)
    {
        var changeMovementArgs = args as ChangeMovementSpeedEventArgs;
        ref var movementStats = ref _movementStatsPool.Get(entity);
        if(changeMovementArgs.RemoveModifier) movementStats.MovementSpeedBonus.Remove(changeMovementArgs.SpeedModifier);
        else movementStats.MovementSpeedBonus.Add(changeMovementArgs.SpeedModifier);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var movementStats = ref _movementStatsPool.Get(entity);
            ref var pBody = ref _physicalBodyPool.Get(entity);
            if (!movementStats.IsMovementAvailable) continue;
            float speedModifier = movementStats.MovementSpeedBonus.Aggregate(1.0f, (a, b) => a * b);
            var targetSpeed = movementStats.MovementSpeed * movementStats.MovementDirection.normalized * speedModifier;
            var runAccelAmount = (50f * movementStats.Acceleration) / movementStats.MovementSpeed * speedModifier;
            var runDeccelAmount = (50f * movementStats.Deceleration) / movementStats.MovementSpeed * speedModifier;
            float accelRate = Mathf.Abs(targetSpeed.magnitude) > 0.01f ? runAccelAmount : runDeccelAmount;
            var speedDiff = targetSpeed - movementStats.Movement;
            var movement = speedDiff * accelRate;
            movementStats.Movement = new Vector2(movementStats.Movement.x + (Time.fixedDeltaTime * movement.x) / pBody.RigidBody.mass,
                                         movementStats.Movement.y + (Time.fixedDeltaTime * movement.y) / pBody.RigidBody.mass);
        }
    }
}
