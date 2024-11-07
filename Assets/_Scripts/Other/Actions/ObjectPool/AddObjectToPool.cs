using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Add object to pool")]
public class AddObjectToPool : GameAction
{
    [SerializeField] private bool _targetIsSender = true;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        int entity = senderEntity;
        if(!_targetIsSender)
        {
            if(takerEntity == null) return;
            entity = takerEntity.Value;
        }
        EcsEventBus.Publish(GameplayEventType.ReturnObjectToPool, entity, null);
    }
}
