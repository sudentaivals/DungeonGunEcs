using System;
using System.Diagnostics;
using CustomAstar;
using Leopotam.EcsLite;
using UnityEngine;

public class NpcToTargetPathSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<PathComponent> _pathPool;

    private EcsPool<NpcTargetComponent> _targetPool;

    private EcsPool<NpcMovement> _npcMovementPool;

    private Astar _astar;

    private readonly int _maxPathsCalculationsPerUpdate = 5;

    private int _currentPathsCalculations = 0;

    private EcsPool<TransformComponent> _transformPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<NpcTargetComponent>()
                       .Inc<PathComponent>()
                       .Inc<TransformComponent>()
                       .Inc<NpcMovement>()
                       .Exc<PooledObjectTag>()
                       .End();
        _pathPool = world.GetPool<PathComponent>();
        _targetPool = world.GetPool<NpcTargetComponent>();
        _transformPool = world.GetPool<TransformComponent>();
        _npcMovementPool = world.GetPool<NpcMovement>();
        //var size = GridManager.Instance.GridSize;
        _astar = new(10000);
        EcsEventBus.Subscribe(GameplayEventType.SetPathfindingStatus, SetPathfindingStatus);
    }

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.SetPathfindingStatus, SetPathfindingStatus);
    }


    private void SetPathfindingStatus(int senderEntity, EventArgs args)
    {
        if(!_pathPool.Has(senderEntity)) return;
        var newArgs = args as ChangePathfindingStatusEventArgs;
        ref var pathfindComponent = ref _pathPool.Get(senderEntity);
        pathfindComponent.IsPathfindWorks = newArgs.IsPathfindingActive;
        pathfindComponent.CurrentDelay = 0;
    }

    public void Run(IEcsSystems systems)
    {
        _currentPathsCalculations = 0;
        foreach (var entity in _filter)
        {
            if(_currentPathsCalculations >= _maxPathsCalculationsPerUpdate) break;

            ref var npcMovementComp = ref _npcMovementPool.Get(entity);
            if (!npcMovementComp.IsMovementActive) continue;

            ref var targetComp = ref _targetPool.Get(entity);
            if(!targetComp.IsTargetFound) continue;

            ref var pathComp = ref _pathPool.Get(entity);
            if(!pathComp.IsPathfindWorks) continue;
            pathComp.CurrentDelay -= Time.fixedDeltaTime;
            if(pathComp.CurrentDelay > 0) continue;

            pathComp.CurrentDelay = pathComp.PathCalculationDelay;


            ref var senderTransform = ref _transformPool.Get(entity);
            ref var targetTransform = ref _transformPool.Get(targetComp.TargetEntity);

            pathComp.Nodes = _astar.FindPath(senderTransform.Transform.position, targetTransform.Transform.position);
            if(pathComp.Nodes != null) pathComp.Nodes.Pop();
            _currentPathsCalculations++;
        }
    }

}
