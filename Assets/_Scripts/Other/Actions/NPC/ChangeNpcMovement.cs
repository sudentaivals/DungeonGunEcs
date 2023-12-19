using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/NPC/Change NPC movement")]
public class ChangeNpcMovement : GameAction
{
    [SerializeField] bool _activateMovement;
    public override void Action(int senderEntity, int? takerEntity)
    {
        var npcMovementStatusArgs = EventArgsObjectPool.GetArgs<SetNpcMovementStatusEventArgs>();
        npcMovementStatusArgs.NewNpcMovementStatus = _activateMovement;
        EcsEventBus.Publish(GameplayEventType.ChangeNpcMovement, senderEntity, npcMovementStatusArgs);
    }

}
