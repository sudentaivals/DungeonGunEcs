using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/NPC/Target is alive or dead")]
public class TargetIsAliveOrDead : BaseGameCondition
{
    [SerializeField] bool _isAlive;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var targetPool = EcsStart.World.GetPool<NpcTargetComponent>();
        ref var npcTarget = ref targetPool.Get(senderEntity);
        if (!npcTarget.IsTargetFound) return false;

        var healthPool = EcsStart.World.GetPool<HealthComponent>();
        ref var healthComp = ref healthPool.Get(npcTarget.TargetEntity);
        return _isAlive ? healthComp.IsAlive == true : healthComp.IsAlive == false;
    }
}
