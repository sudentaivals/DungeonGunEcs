using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/NPC/Npc is facing target")]
public class NpcIsFacingTarget : BaseGameCondition
{
    [SerializeField] bool _isFacingTarget;

    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var npcTargetPool = EcsStart.World.GetPool<NpcTargetComponent>();
        ref var npcTarget = ref npcTargetPool.Get(senderEntity);
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        ref var senderTransform = ref transformPool.Get(senderEntity);
        ref var targetTransform = ref transformPool.Get(npcTarget.TargetEntity);

        var npcToTarget = targetTransform.Transform.position - senderTransform.Transform.position;
        var npcFacing = MathF.Sign(senderTransform.Transform.localScale.x);
        var dot = Vector2.Dot(npcToTarget, npcFacing * Vector2.right);
        if(_isFacingTarget) return dot > 0;
        else return dot <= 0;

    }

}
