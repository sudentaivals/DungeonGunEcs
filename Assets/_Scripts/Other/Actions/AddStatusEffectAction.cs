using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Add status effect")]
public class AddStatusEffectAction : GameAction
{
    [SerializeField] StatusEffectShell _statusEffect;
    [SerializeField] bool _targetIsSender;

    [SerializeField] bool _overrideDuration = false;
    [SerializeField] float _newDuration;

    public override void Action(int senderEntity, int? takerEntity)
    {
        var args = EventArgsObjectPool.GetArgs<AddStatusEffectEventArgs>();
        args.EffectShell = _statusEffect;
        args.OverrideDuration = _overrideDuration;
        args.NewDuration = _newDuration;
        if(_targetIsSender) args.TargetEntity = senderEntity;
        else
        {
            if (!takerEntity.HasValue) return;
            args.TargetEntity = takerEntity.Value;
        }
        EcsEventBus.Publish(GameplayEventType.AddStatusEffect, senderEntity, args);
    }
}
