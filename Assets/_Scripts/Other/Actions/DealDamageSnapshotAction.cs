using AngouriMath;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Deal damage snapshot")]
public class DealDamageSnapshotAction : GameAction
{
    [SerializeField] string DamageFormula;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        if (takerEntity == null) return;
        Entity expr = DamageFormula;
        var damage = (int)expr.EvalNumerical();    
    }
}
