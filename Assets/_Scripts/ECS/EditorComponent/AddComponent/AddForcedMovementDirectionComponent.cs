using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Add component/Add fordec direction movement component")]
public class AddForcedMovementDirectionComponent : ScriptableObject, IAddComponent
{
    private EcsPool<ForcedMovementDirectionComponent> _specialMovementPool;
    
    public void AddComponent(int entity)
    {
        if(_specialMovementPool == null) _specialMovementPool = EcsStart.World.GetPool<ForcedMovementDirectionComponent>();
        if(_specialMovementPool.Has(entity)) return;
        _specialMovementPool.Add(entity);
    }
}
