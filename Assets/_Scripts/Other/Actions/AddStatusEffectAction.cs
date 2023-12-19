using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Add status effect")]
public class AddStatusEffectAction : GameAction
{
    [SerializeField] StatusEffectShell _statusEffect;
    [SerializeField] bool _targetIsSender;

    public override void Action(int senderEntity, int? takerEntity)
    {
        var args = EventArgsObjectPool.GetArgs<AddStatusEffectEventArgs>();
        args.EffectShell = _statusEffect;
        if(_targetIsSender) args.TargetEntity = senderEntity;
        else
        {
            if (!takerEntity.HasValue) return;
            args.TargetEntity = takerEntity.Value;
        }
        EcsEventBus.Publish(GameplayEventType.AddStatusEffect, senderEntity, args);
    }
}
