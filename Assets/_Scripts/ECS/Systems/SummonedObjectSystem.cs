using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedObjectSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter _filter;
    private EcsPool<GlobalStatsComponent> _statsPool;
    private EcsPool<SummonedObjectComponent> _summonedObjectPool;
    private EcsPool<TransformComponent> _transformPool;

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var transformComp = ref _transformPool.Get(entity);
            ref var summonedComp = ref _summonedObjectPool.Get(entity);
            ref var summonedStats = ref _statsPool.Get(entity);
            if (transformComp.Transform.TryGetComponent<SummonedObject>(out var summoned))
            {
                if(summoned.IsMasterEntitySet) continue;
                summonedComp.MasterId = summoned.MasterEntity;
                summonedComp.MasterStats = _statsPool.Get(summoned.MasterEntity);
                summonedStats.DamageBonusFlat = summonedComp.MasterStats.DamageBonusFlat;
                summonedStats.DamageBonusPercent = summonedComp.MasterStats.DamageBonusPercent;
                summonedStats.SpeedBonusPercent = summonedComp.MasterStats.SpeedBonusPercent;
                summonedStats.Faction = summonedComp.MasterStats.Faction;
                summoned.SetMasterEntityState(true);
                //GameObject.Destroy(summoned);
            }
        }
    }
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<SummonedObjectComponent>().End();
        _statsPool = world.GetPool<GlobalStatsComponent>();
        _transformPool = world.GetPool<TransformComponent>();
        _summonedObjectPool = world.GetPool<SummonedObjectComponent>();
    }
}
