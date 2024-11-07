using AngouriMath;
using AngouriMath.Extensions;
using TMPro;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Deal damage")]
public class DealDamageAction : GameAction
{
    [SerializeField] DamageSO _damage;

    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        if (takerEntity == null) return;
        var args = EventArgsObjectPool.GetArgs<DealDamageEventArgs>();
        args.InitialDamage = _damage.GetDamageValue(senderEntity, takerEntity);
        args.DamageType = _damage.DamageType;
        args.DamageId = _damage.Guid;
        args.DamageTakerEntity = takerEntity.Value;

        EcsEventBus.Publish(GameplayEventType.DealDamage, senderEntity, args);
    }
}

