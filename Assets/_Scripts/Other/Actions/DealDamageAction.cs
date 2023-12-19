using AngouriMath;
using AngouriMath.Extensions;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Deal damage")]
public class DealDamageAction : GameAction
{
    [SerializeField] int _damage;

    public override void Action(int senderEntity, int? takerEntity)
    {
        if (takerEntity == null) return;
        EcsEventBus.Publish(GameplayEventType.DealDamage, senderEntity, new DealDamageEventArgs(takerEntity.Value, _damage, null, senderEntity));
    }
}

