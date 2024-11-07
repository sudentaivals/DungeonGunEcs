using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/Sender is alive or dead")]
public class SenderIsAliveOrDead : BaseGameCondition
{
    [SerializeField] bool _isAlive;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var healthPool = EcsStart.World.GetPool<HealthComponent>();
        ref var healthComp = ref healthPool.Get(senderEntity);
        return _isAlive ? healthComp.IsAlive == true : healthComp.IsAlive == false;
    }
}
