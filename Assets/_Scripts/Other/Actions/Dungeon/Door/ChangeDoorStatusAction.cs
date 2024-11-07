using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Dungeon/Change door status")]
public class ChangeDoorStatusAction : GameAction
{
    [SerializeField] private bool _isOpen;
    [SerializeField] private bool _targetIsSender = true;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        int targetEntity = senderEntity;
        if(!_targetIsSender)
        {
            if(takerEntity.HasValue) targetEntity = takerEntity.Value;
            else return;
        }
        
        var args = EventArgsObjectPool.GetArgs<ChangeColliderStatusEventArgs>();
        args.NewStatus = _isOpen;
        EcsEventBus.Publish(GameplayEventType.ChangeDoorStatus, targetEntity, args);
    }
}
