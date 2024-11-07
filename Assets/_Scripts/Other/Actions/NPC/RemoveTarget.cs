using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/NPC/Remove target")]
public class RemoveTarget : GameAction
{
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var targetPool = EcsStart.World.GetPool<NpcTargetComponent>();
        ref var npcTarget = ref targetPool.Get(senderEntity);

    }

}
