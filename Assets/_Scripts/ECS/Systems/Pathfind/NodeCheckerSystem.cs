using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

public class NodeCheckerSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<PathComponent> _pathPool;
    private EcsPool<TransformComponent> _transformPool;

    private EcsPool<NpcMovement> _npcMovementPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PathComponent>().Inc<TransformComponent>()
                                               .Exc<PooledObjectTag>()
                                               .End();
        _pathPool = world.GetPool<PathComponent>();
        _npcMovementPool = world.GetPool<NpcMovement>();
        _transformPool = world.GetPool<TransformComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var npcMovementComp = ref _npcMovementPool.Get(entity);
            if(!npcMovementComp.IsMovementActive) continue;
            ref var pathComp = ref _pathPool.Get(entity);
            if(pathComp.Nodes == null) continue;
            if(pathComp.Nodes.Count == 0) continue;

            ref var transformComp = ref _transformPool.Get(entity);
            var nodePos = pathComp.Nodes.First().WorldPosition;
            var senderPos = transformComp.Transform.position;
            if(Vector2.Distance(senderPos, nodePos) < pathComp.NodeRemoveEpsilon) pathComp.Nodes.Pop();

        }
    }
}
