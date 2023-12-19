using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Movement direction/Simple")]
public class SimpleMovementDirection : ScriptableObject, IMovementDirection
{
    EcsPool<TransformComponent> _transformPool;

    public Vector2 GetDirection(int sender)
    {
        if(_transformPool == null) _transformPool = EcsStart.World.GetPool<TransformComponent>();

        ref var transform = ref _transformPool.Get(sender);

        return transform.Transform.right;

    }
}

