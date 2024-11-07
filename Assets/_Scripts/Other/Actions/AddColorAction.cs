using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Add color")]

public class AddColorAction : GameAction
{
    [SerializeField] Color _color;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        if (takerEntity == null) return;
        var args = EventArgsObjectPool.GetArgs<AddColorEventArgs>();
        args.Color = _color;
        args.TakerEntity = takerEntity.Value;
        EcsEventBus.Publish(GameplayEventType.AddColor, senderEntity, args);
    }
}
