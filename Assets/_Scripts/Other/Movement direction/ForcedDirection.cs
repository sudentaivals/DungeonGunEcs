using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Movement direction/Forced direction")]
public class ForcedDirection : ScriptableObject, IMovementDirection
{
    private EcsPool<ForcedMovementDirectionComponent> _specialMovementPool;
    public Vector2 GetDirection(int sender)
    {
        if(_specialMovementPool == null) _specialMovementPool = EcsStart.World.GetPool<ForcedMovementDirectionComponent>();
        if(!_specialMovementPool.Has(sender)) return Vector2.zero;
        ref var specialMovement = ref _specialMovementPool.Get(sender);
        return specialMovement.Direction;
    }
}

