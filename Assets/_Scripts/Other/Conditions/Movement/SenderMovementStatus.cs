using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/Sender movement status")]
public class SenderMovementStatus : BaseGameCondition
{
    [SerializeField] bool _movementIsAvailable;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var movementStatsPool = EcsStart.World.GetPool<MovementStatsComponent>();
        ref var stats = ref movementStatsPool.Get(senderEntity);
        if (_movementIsAvailable) return stats.IsMovementAvailable == true;
        else return stats.IsMovementAvailable == false;
    }
}
