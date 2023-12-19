using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/PositioType/Target position")]
public class TargetPosition : ScriptableObject, IPositionType
{
    public Vector2 GetPosition(int sender, int? taker)
    {
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        ref var hostTransform = ref transformPool.Get(sender);
        if(taker == null) return (Vector2)hostTransform.Transform.position + Vector2.right;
        ref var takerTransform = ref transformPool.Get(taker.Value);
        return takerTransform.Transform.position;

    }
}
