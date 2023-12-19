using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/RotationType/Towards npc target")]
public class RotateToNpcTarget : ScriptableObject, IRotationType
{
    public Quaternion GetRotation(int sender, int? taker)
    {
        var world = EcsStart.World;
        var npcTargetPool = world.GetPool<NpcTargetComponent>();
        ref var npcTarget = ref npcTargetPool.Get(sender);
        if(!npcTarget.IsTargetFound) return Quaternion.identity;
        var transformPool = world.GetPool<TransformComponent>();
        ref var targetTransform = ref transformPool.Get(npcTarget.TargetEntity);
        ref var senderTransform = ref transformPool.Get(sender);
        var senderToTarget = targetTransform.Transform.position - senderTransform.Transform.position;
        var angle = Mathf.Atan2(senderToTarget.y, senderToTarget.x);
        var angleDeg = angle * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angleDeg);

    }
}
