using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Add sender to cinemachine group")]
public class AddSenderToCinemachineGroupAction : GameAction
{
    [SerializeField] float _weight;
    [SerializeField] float _radius;

    [SerializeField] bool _remove;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        if(_remove)
        {
            EcsEventBus.Publish(GameplayEventType.RemoveSenderFromCinemachineGroup, senderEntity, null);
            return;
        }
        var args = EventArgsObjectPool.GetArgs<AddSenderToCinemachineTargetGroupEventArgs>();
        args.Weight = _weight;
        args.Radius = _radius;
        EcsEventBus.Publish(GameplayEventType.AddSenderToCinemachineGroup, senderEntity, args);
    }
}
