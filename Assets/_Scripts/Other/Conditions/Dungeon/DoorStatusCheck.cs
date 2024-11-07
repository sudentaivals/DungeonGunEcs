using Leopotam.EcsLite;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Conditions/Dungeon/Door status check")]
public class DoorStatusCheck : BaseGameCondition
{
    [SerializeField] private bool _isOpen;
    [SerializeField] private bool _targetIsSender = true;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var doorPool = EcsStart.World.GetPool<DoorComponent>();
        int targetEntity = senderEntity;
        if(!_targetIsSender)
        {
            if(takerEntity == null) return false;
            if(!takerEntity.HasValue) return false;
            targetEntity = takerEntity.Value;
        }
        if(!doorPool.Has(targetEntity)) return false;
        ref var doorComponent = ref doorPool.Get(targetEntity);
        return _isOpen ? doorComponent.IsOpen == true : doorComponent.IsOpen == false;
    }
}
