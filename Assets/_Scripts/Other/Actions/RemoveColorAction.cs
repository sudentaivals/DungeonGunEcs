using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Remove color")]
public class RemoveColorAction : GameAction
{
    [SerializeField] Color _color;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        if (takerEntity == null) return;
        var args = EventArgsObjectPool.GetArgs<RemoveColorEventArgs>();
        args.Color = _color;
        args.TakerEntity = takerEntity.Value;
        EcsEventBus.Publish(GameplayEventType.RemoveColor, senderEntity, args);
    }

}
