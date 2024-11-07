using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/NPC/Target state")]
public class TargetState : BaseGameCondition
{
    [SerializeField] bool _targetExists;

    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var targetPool = EcsStart.World.GetPool<NpcTargetComponent>();
        ref var npcTarget = ref targetPool.Get(senderEntity);
        if (_targetExists)
        {
            return npcTarget.IsTargetFound;
        }
        else
        {
            return !npcTarget.IsTargetFound;
        }
    }
}
