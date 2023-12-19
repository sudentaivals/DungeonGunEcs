using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Movement direction/Npc to node")]
public class NpcToClosestNodeDirection : ScriptableObject, IMovementDirection
{
    private EcsPool<NpcMovement> _npcMovementPool;
    private EcsPool<PathComponent> _pathPool;
    private EcsPool<TransformComponent> _transformPool;

    public Vector2 GetDirection(int sender)
    {
        if(_npcMovementPool == null) _npcMovementPool = EcsStart.World.GetPool<NpcMovement>();
        if(_pathPool == null) _pathPool = EcsStart.World.GetPool<PathComponent>();
        if(_transformPool == null) _transformPool = EcsStart.World.GetPool<TransformComponent>();

        if(!_pathPool.Has(sender)) return Vector2.zero;
        if(!_npcMovementPool.Has(sender)) return Vector2.zero;

        ref var npcMovementComp = ref _npcMovementPool.Get(sender);
        if (!npcMovementComp.IsMovementActive) return Vector2.zero;
        
        ref var pathComp = ref _pathPool.Get(sender);
        if(pathComp.Nodes == null) return Vector2.zero;
        if(pathComp.Nodes.Count == 0) return Vector2.zero;
        
        ref var transformComp = ref _transformPool.Get(sender);
        var senderPosition = transformComp.Transform.position;
        var nodePosition = pathComp.Nodes.Peek().WorldPosition;

        return (nodePosition - senderPosition).normalized;
    }
}

