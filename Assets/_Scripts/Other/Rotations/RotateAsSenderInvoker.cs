using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/RotationType/As sender invoker")]
public class RotateAsSenderInvoker : ScriptableObject, IRotationType
{
    public Quaternion GetRotation(int sender, int? taker)
    {
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        var invokerPool = EcsStart.World.GetPool<InvokerComponent>();
        Vector2 senderToSenderInvoker = (invokerPool.Get(sender).InvokerPosition.position - transformPool.Get(sender).Transform.position).normalized;
        var angle = Mathf.Rad2Deg * Mathf.Atan2(senderToSenderInvoker.y, senderToSenderInvoker.x);
        return Quaternion.Euler(0, 0, angle);
    }
}
