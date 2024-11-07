using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Conditions/Taker is alive or dead")]
public class TakerIsAliveOrDead : BaseGameCondition
{
    [SerializeField] bool _isAlive;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var healthPool = EcsStart.World.GetPool<HealthComponent>();
        if (!healthPool.Has(takerEntity.Value)) return false;
        ref var healthComp = ref healthPool.Get(takerEntity.Value);
        return _isAlive ? healthComp.IsAlive == true : healthComp.IsAlive == false;
    }
}
