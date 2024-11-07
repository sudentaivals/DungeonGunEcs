using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Change collider status")]
public class ChangeColliderStatusAction : GameAction
{
    [SerializeField] private bool _enableCollider;
    [SerializeField] private bool _targetIsSender = true;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        int targetEntity = senderEntity;
        if(!_targetIsSender)
        {
            if(takerEntity == null) return;
            if(!takerEntity.HasValue) return;
            targetEntity = takerEntity.Value;
        }
        var args = EventArgsObjectPool.GetArgs<ChangeStatusEventArgs>();
        args.NewStatus = _enableCollider;
        EcsEventBus.Publish(GameplayEventType.ChangeColliderStatus, targetEntity, args);
    }
}
