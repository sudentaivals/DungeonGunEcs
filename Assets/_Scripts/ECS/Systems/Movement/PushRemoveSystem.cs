using Leopotam.EcsLite;
using System;
using UnityEngine;

public class PushRemoveSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<PushStatsComponent> _pushStatsPool;
    private EcsPool<PhysicalBodyComponent> _physicalBodyPool;
    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _pushStatsPool = null;
        _physicalBodyPool = null;
        EcsEventBus.Unsubscribe(GameplayEventType.RemovePush, RemovePush);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PushStatsComponent>()
                       .Inc<PhysicalBodyComponent>()
                       .Exc<PooledObjectTag>()
                       .End();
        _pushStatsPool = world.GetPool<PushStatsComponent>();
        _physicalBodyPool = world.GetPool<PhysicalBodyComponent>();
        EcsEventBus.Subscribe(GameplayEventType.RemovePush, RemovePush);
    }

    private void RemovePush(int entity, EventArgs args)
    {
        ref var stats = ref _pushStatsPool.Get(entity);
        stats.Push = Vector2.zero;
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var pushStats = ref _pushStatsPool.Get(entity);
            ref var physicalBody = ref _physicalBodyPool.Get(entity);
            var newPushX = pushStats.Push.x >= 0
                ? Mathf.Clamp(pushStats.Push.x + (-Physics2D.gravity.magnitude * Time.fixedDeltaTime) / physicalBody.RigidBody.mass * pushStats.Friction, 0, float.MaxValue)
                : Mathf.Clamp(pushStats.Push.x + (Physics2D.gravity.magnitude * Time.fixedDeltaTime) / physicalBody.RigidBody.mass * pushStats.Friction, float.MinValue, 0);
            var newPushY = pushStats.Push.y >= 0
                ? Mathf.Clamp(pushStats.Push.y + (-Physics2D.gravity.magnitude * Time.fixedDeltaTime) / physicalBody.RigidBody.mass * pushStats.Friction, 0, float.MaxValue)
                : Mathf.Clamp(pushStats.Push.y + (Physics2D.gravity.magnitude * Time.fixedDeltaTime) / physicalBody.RigidBody.mass * pushStats.Friction, float.MinValue, 0);
            pushStats.Push = new Vector2(newPushX, newPushY);
        }
    }

}
