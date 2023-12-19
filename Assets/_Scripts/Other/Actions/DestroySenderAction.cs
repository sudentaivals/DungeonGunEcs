using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Destroy sender")]
public class DestroySenderAction : GameAction
{
    public override void Action(int senderEntity, int? takerEntity)
    {
        EcsEventBus.Publish(GameplayEventType.DestroyObject, senderEntity, null);
    }
}
