using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Remove rotation")]
public class RemoveRotationAction : GameAction
{
    [SerializeField] bool _objectIsSender;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        if(_objectIsSender) EcsEventBus.Publish(GameplayEventType.ResetRotation, senderEntity, null);
        else
        {
            if(takerEntity == null) return;
            EcsEventBus.Publish(GameplayEventType.ResetRotation, takerEntity.Value, null);
        }
    }
}
