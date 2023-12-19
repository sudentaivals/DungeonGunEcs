using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/PositioType/Caster position")]
public class CasterPosition : ScriptableObject, IPositionType
{
    public Vector2 GetPosition(int sender, int? taker)
    {
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        ref var hostTransform = ref transformPool.Get(sender);
        return hostTransform.Transform.position;
    }
}
