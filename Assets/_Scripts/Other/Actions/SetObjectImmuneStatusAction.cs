using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Set object immune status")]
public class SetObjectImmuneStatusAction : GameAction
{
    [SerializeField] bool _setImmune;
    [SerializeField] bool _isSender;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        int entity = -1;
        if(_isSender) entity = senderEntity;
        else
        {
            if(takerEntity == null) return;
            entity = takerEntity.Value;
        }
        var args = EventArgsObjectPool.GetArgs<ChangeImmuneStatusEventArgs>();
        args.IsImmune = _setImmune;
        EcsEventBus.Publish(GameplayEventType.SetImmuneStatus, entity, args);
    }
}
