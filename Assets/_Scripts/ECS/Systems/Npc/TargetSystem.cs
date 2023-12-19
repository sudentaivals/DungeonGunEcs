using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter _filter;
    private EcsPool<TransformComponent> _transformPool;
    private EcsPool<NpcTargetComponent> _npcTargetPool;
    private float _minDelayTimer;
    private float _maxDelayTimer;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<NpcTargetComponent>().Exc<PooledObjectTag>().End();
        _transformPool = world.GetPool<TransformComponent>();
        _npcTargetPool = world.GetPool<NpcTargetComponent>();
        _minDelayTimer = 0.1f;
        _maxDelayTimer = 1f;
        //EcsEventBus.Subscribe(GameplayEventType.FindNewTarget, FindTarget);
        //EcsEventBus.Subscribe(GameplayEventType.RemoveTarget, RemoveTarget);

    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            FindTarget(entity);
            CheckTargetStatus(entity);
        }
    }

    private void FindTarget(int entity)
    {
        ref var senderNpcTarget = ref _npcTargetPool.Get(entity);
        if (senderNpcTarget.IsTargetFound) return;

        if (senderNpcTarget.FindTargetDelay > 0)
        {
            senderNpcTarget.FindTargetDelay -= Time.fixedDeltaTime;
        }
        else
        {
            ref var senderTransform = ref _transformPool.Get(entity);
            senderNpcTarget.FindTargetDelay = UnityEngine.Random.Range(_minDelayTimer, _maxDelayTimer);
            var entities = PhysicsHelper.GetAllEntitiesInRadius(senderTransform.Transform.position, senderNpcTarget.TargetRadius);
            if (entities.Count == 0) return;
            var targetCondition = senderNpcTarget.TargetCondition;
            var filteredEntities = entities.Where(filteredEntety => targetCondition.CheckCondition(entity, filteredEntety));// && targetCondition.Select(condition => condition.CheckCondition(entity, filteredEntety)).All(b => b == true)).ToList();
            if (!filteredEntities.Any()) return;
            var senderPos = senderTransform.Transform.position;
            var closestEntity = filteredEntities.OrderBy(a => (_transformPool.Get(a).Transform.position - senderPos).magnitude).First();
            senderNpcTarget.IsTargetFound = true;
            senderNpcTarget.TargetEntity = closestEntity;
        }
    }

    private void CheckTargetStatus(int entity)
    {
        ref var npcTargetComponent = ref _npcTargetPool.Get(entity);
        if (!npcTargetComponent.IsTargetFound) return;
        ref var senderTransform = ref _transformPool.Get(entity);
        ref var targetTransform = ref _transformPool.Get(npcTargetComponent.TargetEntity);
        int target = npcTargetComponent.TargetEntity;
        var isTargetConditionsValid = npcTargetComponent.TargetCondition.CheckCondition(entity, target);//.Select(a => a.CheckCondition(entity, target)).All(b => b == true);
        //Debug.Log($"isTargetConditionsValid: {isTargetConditionsValid}");
        //add range check
        var isTargetInRange = (senderTransform.Transform.position - targetTransform.Transform.position).magnitude < npcTargetComponent.TargetRadius;
        //Debug.Log($"isTargetInRange: {isTargetInRange}");
        if (isTargetConditionsValid && isTargetInRange) return;
        RemoveTarget(entity);
    }

    private void RemoveTarget(int entity)
    {
        ref var npcTarget = ref _npcTargetPool.Get(entity);
        npcTarget.IsTargetFound = false;
    }
}
