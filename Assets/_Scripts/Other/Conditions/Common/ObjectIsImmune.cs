using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(menuName = "My Assets/Conditions/Is object immune")]
public class ObjectIsImmune : BaseGameCondition
{
    [SerializeField] bool _isImmune;
    [SerializeField] bool _isSender;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        int entity = -1;
        if(_isSender) entity = senderEntity;
        else
        {
            if(takerEntity == null) return false;
            entity = takerEntity.Value;
        }
        var globalStatsPool = EcsStart.World.GetPool<GlobalStatsComponent>();
        ref var globalStats = ref globalStatsPool.Get(entity);
        if(_isImmune) return globalStats.IsImmune == true;
        else return globalStats.IsImmune == false;
    }
}
