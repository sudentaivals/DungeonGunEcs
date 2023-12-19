using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Remove component/Remove forced direction component")]
public class RemoveForcedDirectionMovementComponent : ScriptableObject, IRemoveComponent
{
    private EcsPool<ForcedMovementDirectionComponent> _specialMovementPool;

    public void RemoveComponent(int entity)
    {
        if(_specialMovementPool == null) _specialMovementPool = EcsStart.World.GetPool<ForcedMovementDirectionComponent>();
        if(!_specialMovementPool.Has(entity)) return;
        _specialMovementPool.Del(entity);
    }
}
