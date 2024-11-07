using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/Taker friendly checker")]
public class TakerIsFriendlyChecker : BaseGameCondition
{
    [SerializeField] bool _isFriendly;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        if (!takerEntity.HasValue) return false;
        var statsPool = EcsStart.World.GetPool<GlobalStatsComponent>();
        ref var senderStats = ref statsPool.Get(senderEntity);
        ref var takerStats = ref statsPool.Get(takerEntity.Value);
        if (_isFriendly) return senderStats.Faction == takerStats.Faction;
        if (!_isFriendly) return senderStats.Faction != takerStats.Faction;
        return false;
    }
}
