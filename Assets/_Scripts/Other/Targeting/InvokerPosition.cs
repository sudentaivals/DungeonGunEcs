using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/PositioType/Invoker position")]
public class InvokerPosition : ScriptableObject, IPositionType
{
    public Vector2 GetPosition(int sender, int? taker)
    {
        var invokerPool = EcsStart.World.GetPool<InvokerComponent>();
        var position = Vector3.zero;
        if (!invokerPool.Has(sender)) return Vector3.zero;
        ref var invoker = ref invokerPool.Get(sender);
        return invoker.InvokerPosition.position;
    }
}
