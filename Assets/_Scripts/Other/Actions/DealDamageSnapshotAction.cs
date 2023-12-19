using AngouriMath;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Deal damage snapshot")]
public class DealDamageSnapshotAction : GameAction
{
    [SerializeField] string DamageFormula;
    public override void Action(int senderEntity, int? takerEntity)
    {
        if (takerEntity == null) return;
        Entity expr = DamageFormula;
        var damage = (int)expr.EvalNumerical();
        EcsEventBus.Publish(GameplayEventType.DealDamage, senderEntity, new DealDamageEventArgs(takerEntity.Value, damage, null, null));
    }
}
