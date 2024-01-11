using Leopotam.EcsLite;
using System;
using UnityEngine;

public class CombineMovementSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<MovementStatsComponent> _movementStatsPool;
    private EcsPool<PushStatsComponent> _pushStatsPool;
    private EcsPool<PhysicalBodyComponent> _physicalBodyPool;
    
    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _movementStatsPool = null;
        _physicalBodyPool = null;
        EcsEventBus.Unsubscribe(GameplayEventType.AddPush, AddPush);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PhysicalBodyComponent>()
                       .Exc<PooledObjectTag>()
                       .End();
        _movementStatsPool = world.GetPool<MovementStatsComponent>();
        _physicalBodyPool = world.GetPool<PhysicalBodyComponent>();
        _pushStatsPool = world.GetPool<PushStatsComponent>();
        EcsEventBus.Subscribe(GameplayEventType.AddPush, AddPush);
    }

    private void AddPush(int entity, EventArgs args)
    {
        var pushArgs = args as AddPushEventArgs;
        ref var pushStats = ref _pushStatsPool.Get(entity);
        var newPush = pushArgs.Direction * pushArgs.PushPower * (1f - pushStats.PushResistance);
        ref var physicalBodyComponent = ref _physicalBodyPool.Get(entity);
        float timeMod = pushArgs.IsImpulse ? 1f : Time.fixedDeltaTime;
        pushStats.Push = new Vector2(
                                 pushStats.Push.x + timeMod * newPush.x / physicalBodyComponent.RigidBody.mass,
                                 pushStats.Push.y + timeMod * newPush.y / physicalBodyComponent.RigidBody.mass);
    }

    private Vector2 GetCurrentPush(int entity)
    {
        Vector2 push = Vector2.zero;
        if(_pushStatsPool.Has(entity))
        {
            ref var pushStats = ref _pushStatsPool.Get(entity);
            push = pushStats.Push;
        }
        return push;
    }

    private Vector2 GetCurrentMovement(int entity)
    {
        Vector2 movement = Vector2.zero;
        if(_movementStatsPool.Has(entity))
        {
            ref var movementStats = ref _movementStatsPool.Get(entity);
            movement = movementStats.Movement;
        }
        return movement;
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            var push = GetCurrentPush(entity);
            var movement = GetCurrentMovement(entity);
            ref var physicalBody = ref _physicalBodyPool.Get(entity);
            //push
            physicalBody.RigidBody.velocity = push + movement;
        }
    }

}
