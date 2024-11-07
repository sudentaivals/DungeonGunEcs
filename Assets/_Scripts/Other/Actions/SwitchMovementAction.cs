using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Change movement availability")]
public class SwitchMovementAction : GameAction
{
    [SerializeField] bool _setMovementAvailable;
    [SerializeField] bool _targetIsSender;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var setMovementArgs = EventArgsObjectPool.GetArgs<SetMovementEventArgs>();
        setMovementArgs.NewMovementStatus = _setMovementAvailable;
        if (_targetIsSender)
        {
            EcsEventBus.Publish(GameplayEventType.SetMovementStatus, senderEntity, setMovementArgs);
        }
        else
        {
            if (!takerEntity.HasValue) return;
            EcsEventBus.Publish(GameplayEventType.SetMovementStatus, takerEntity.Value, setMovementArgs);
        }
    }
}
