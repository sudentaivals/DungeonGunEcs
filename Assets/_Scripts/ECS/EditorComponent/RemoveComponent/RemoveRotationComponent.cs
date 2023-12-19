using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Remove component/Remove rotation component")]
public class RemoveRotationComponent : ScriptableObject, IRemoveComponent
{
    private EcsPool<RotationComponent> _rotationPool;
    public void RemoveComponent(int entity)
    {
        if(_rotationPool == null) _rotationPool = EcsStart.World.GetPool<RotationComponent>();
        if(!_rotationPool.Has(entity)) return;
        _rotationPool.Del(entity);
    }
}
