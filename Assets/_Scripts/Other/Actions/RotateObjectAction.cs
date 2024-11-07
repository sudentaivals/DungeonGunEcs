using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Rotate object")]
public class RotateObjectAction : GameAction
{
    [SerializeField] bool _objectIsSender;
    [SerializeField] float _rotationValue;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var args = EventArgsObjectPool.GetArgs<RotateObjectEventArgs>();
        args.RotationValue = _rotationValue;
        if(_objectIsSender) EcsEventBus.Publish(GameplayEventType.RotateObject, senderEntity, args);
        else
        {
            if(takerEntity == null) return;
            EcsEventBus.Publish(GameplayEventType.RotateObject, takerEntity.Value, args);
        }
    }
}
