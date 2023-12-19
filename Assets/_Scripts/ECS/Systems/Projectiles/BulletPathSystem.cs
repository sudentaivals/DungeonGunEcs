using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPathSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<BulletStatsComponent>().Exc<PooledObjectTag>().End();
        var bulletStatsPool = world.GetPool<BulletStatsComponent>();
        var transformPool = world.GetPool<TransformComponent>();
        foreach (int entity in filter)
        {
            ref var bulletStats = ref bulletStatsPool.Get(entity);
            ref var bulletTransform = ref transformPool.Get(entity);
            if (!bulletStats.IsActivated) continue;
            if(bulletStats.CurrentFlyDistance >= bulletStats.MaxFlyDistance)
            {
                //kill bullet
                bulletStats.ActionsOnReachMaxFlyDistance.Action(entity, null);
                //EcsEventBus.Publish(GameplayEventType.DestroyObject, entity, null);
            }
            else
            {
                var distance = (bulletStats.OldPosition - bulletTransform.Transform.position).magnitude;
                bulletStats.CurrentFlyDistance += distance;
                bulletStats.OldPosition = bulletTransform.Transform.position;
            }
        }
    }
}
