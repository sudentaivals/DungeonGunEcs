using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/NPC/Target range check")]
public class TargetIsOutOfRange : BaseGameCondition
{
    [SerializeField] bool _targetInRange;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        var targetPool = EcsStart.World.GetPool<NpcTargetComponent>();
        ref var npcTarget = ref targetPool.Get(senderEntity);
        ref var senderTransform = ref transformPool.Get(senderEntity);
        ref var targetTransform = ref transformPool.Get(npcTarget.TargetEntity);
        if (_targetInRange)
        {
            return (targetTransform.Transform.position - senderTransform.Transform.position).magnitude < npcTarget.TargetRadius;
        }
        else
        {
            return (targetTransform.Transform.position - senderTransform.Transform.position).magnitude > npcTarget.TargetRadius;
        }
    }
}
