using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/RotationType/From target to sender")]
public class RotateFromTargetToSender : ScriptableObject, IRotationType
{
    public Quaternion GetRotation(int sender, int? taker)
    {
        if(taker == null) return Quaternion.identity;
        var world = EcsStart.World;
        var transformPool = world.GetPool<TransformComponent>();
        ref var takerTransform = ref transformPool.Get(taker.Value);
        ref var senderTransform = ref transformPool.Get(sender);
        var targetToSender = senderTransform.Transform.position - takerTransform.Transform.position;
        var angle = Mathf.Atan2(targetToSender.y, targetToSender.x);
        var angleDeg = angle * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angleDeg);
    }
}
