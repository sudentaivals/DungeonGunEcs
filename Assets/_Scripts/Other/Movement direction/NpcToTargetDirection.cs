using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Movement direction/Npc to target")]
public class NpcToTargetDirection : ScriptableObject, IMovementDirection
{
    private EcsPool<NpcMovement> _npcMovementPool;
    private EcsPool<NpcTargetComponent> _npcTargetPool;
    private EcsPool<TransformComponent> _transformPool;
    public Vector2 GetDirection(int sender)
    {
        if(_npcMovementPool == null) _npcMovementPool = EcsStart.World.GetPool<NpcMovement>();
        if(_npcTargetPool == null) _npcTargetPool = EcsStart.World.GetPool<NpcTargetComponent>();
        if(_transformPool == null) _transformPool = EcsStart.World.GetPool<TransformComponent>();

        if(!_npcTargetPool.Has(sender)) return Vector2.zero;
        if(!_npcMovementPool.Has(sender)) return Vector2.zero;

        ref var npcMovement = ref _npcMovementPool.Get(sender);
        if (!npcMovement.IsMovementActive) return Vector2.zero;

        ref var npcTarget = ref _npcTargetPool.Get(sender);
        if(!npcTarget.IsTargetFound)
        {
            return Vector2.zero;
        }
        else
        {
            ref var targetTransform = ref _transformPool.Get(npcTarget.TargetEntity);
            ref var entityTransform = ref _transformPool.Get(sender);
            return (targetTransform.Transform.position - entityTransform.Transform.position).normalized;
        }

    }
}

