using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

public class NpcToClosestNodeDirectionSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter _filter;
    private EcsPool<MovementStatsComponent> _movementStatsPool;
    private EcsPool<TransformComponent> _transformPool;
    private EcsPool<PathComponent> _pathPool;
    private EcsPool<NpcMovement> _npcMovementPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<GlobalStatsComponent>().Inc<PathComponent>()
                                                .Inc<NodeDirectionMovementTag>()
                                                .Inc<NpcMovement>()
                                                .Exc<PooledObjectTag>()
                                                .Exc<TargetDirectionMovementTag>()
                                                .End();
        _movementStatsPool = world.GetPool<MovementStatsComponent>();
        _transformPool = world.GetPool<TransformComponent>();
        _pathPool = world.GetPool<PathComponent>();
        _npcMovementPool = world.GetPool<NpcMovement>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var statsComp = ref _movementStatsPool.Get(entity);
            statsComp.MovementDirection = GetDirection(entity);
        }
    }

    private Vector2 GetDirection(int entity)
    {
        ref var npcMovementComp = ref _npcMovementPool.Get(entity);
        if (!npcMovementComp.IsMovementActive) return Vector2.zero;
        
        ref var pathComp = ref _pathPool.Get(entity);
        if(pathComp.Nodes == null) return Vector2.zero;
        if(pathComp.Nodes.Count == 0) return Vector2.zero;
        
        ref var transformComp = ref _transformPool.Get(entity);
        var senderPosition = transformComp.Transform.position;
        var nodePosition = pathComp.Nodes.First().WorldPosition;

        return (nodePosition - senderPosition).normalized;
    }
}
