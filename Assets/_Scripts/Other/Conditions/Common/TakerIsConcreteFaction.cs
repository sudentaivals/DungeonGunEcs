using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/Taker is concrete faction")]
public class TakerIsConcreteFaction : BaseGameCondition
{
    [SerializeField] Faction _faction;

    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        if (!takerEntity.HasValue) return false;
        var statsPool = EcsStart.World.GetPool<GlobalStatsComponent>();
        ref var takerStats = ref statsPool.Get(takerEntity.Value);
        return takerStats.Faction == _faction;
    }
}
