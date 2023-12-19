using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Add component/Add rotation component")]
public class AddRotationComponent : ScriptableObject, IAddComponent
{
    private EcsPool<RotationComponent> _rotationPool;

    public void AddComponent(int entity)
    {
        if(_rotationPool == null) _rotationPool = EcsStart.World.GetPool<RotationComponent>();
        if(_rotationPool.Has(entity)) return;
        _rotationPool.Add(entity);
    }
}
