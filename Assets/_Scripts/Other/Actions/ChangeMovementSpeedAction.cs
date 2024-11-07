using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Change movement speed")]
public class ChangeMovementSpeedAction : GameAction
{
    [SerializeField] float _speedModifier;
    [SerializeField] bool _removeModifier;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var args = EventArgsObjectPool.GetArgs<ChangeMovementSpeedEventArgs>();
        args.SpeedModifier = _speedModifier;
        args.RemoveModifier = _removeModifier;
        EcsEventBus.Publish(GameplayEventType.ChangeMovementSpeed, senderEntity, args);
    }
}
