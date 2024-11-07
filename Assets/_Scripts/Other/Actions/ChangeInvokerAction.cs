using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Change invoker")]
public class ChangeInvokerAction : GameAction
{
    [SerializeField] Sprite _newInvokerSprite;
    [SerializeField] Vector2 _newInvokerPosition;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        ref var invokerComponent = ref EcsStart.World.GetPool<InvokerComponent>().Get(senderEntity);
        invokerComponent.InvokerPosition.localPosition = _newInvokerPosition;
        invokerComponent.InvokerSprite.sprite = _newInvokerSprite;
    }
}
