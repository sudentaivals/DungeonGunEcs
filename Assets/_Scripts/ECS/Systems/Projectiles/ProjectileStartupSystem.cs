using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStartupSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter _filter;
    private EcsPool<BulletStatsComponent> _bulletStatsPool;
    private EcsPool<TransformComponent> _transformPool;
    private EcsPool<GlobalStatsComponent> _statsPool;
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<BulletStatsComponent>().Exc<PooledObjectTag>().End();
        _bulletStatsPool = world.GetPool<BulletStatsComponent>();
        _statsPool = world.GetPool<GlobalStatsComponent>();
        _transformPool = world.GetPool<TransformComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var bulletStats = ref _bulletStatsPool.Get(entity);
            if (bulletStats.IsActivated) continue;
            ref var stats = ref _statsPool.Get(entity);
            ref var transform = ref _transformPool.Get(entity);
            //set old pos
            bulletStats.OldPosition = transform.Transform.position;
            //activate
            bulletStats.IsActivated = true;
        }
    }
}
