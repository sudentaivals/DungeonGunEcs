using AngouriMath;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Deal damage snapshot in radius")]
public class DealDamageSnapshotInRangeAction : GameAction
{
    [SerializeField] string DamageFormula;
    [SerializeField] float _radius;

    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        /*//convert dmg into formula
        Entity expr = DamageFormula;
        var damage = (int)expr.EvalNumerical();
        var statsPool = EcsStart.World.GetPool<StatsComponent>();
        var enemyEntitiesInRange = PhysicsHelper.GetAllEntitiesInRadius(senderEntity, args.Position, _radius);
        foreach (var otherEntity in enemyEntitiesInRange)
        {
            ref var otheStats = ref statsPool.Get(otherEntity);
            ref var senderStats = ref statsPool.Get(senderEntity);
            switch (_fireSelection)
            {
                case FactionSelection.All:
                    EcsEventBus.Publish(GameplayEventType.DealDamage, senderEntity, new DealDamageEventArgs(otherEntity, damage, args.SenderStats, null));
                    break;
                case FactionSelection.Friendly:
                    if (otheStats.Faction == senderStats.Faction)
                    {
                        EcsEventBus.Publish(GameplayEventType.DealDamage, senderEntity, new DealDamageEventArgs(otherEntity, damage, args.SenderStats, senderEntity));
                    }
                    break;
                case FactionSelection.Enemy:
                    if (otheStats.Faction != senderStats.Faction)
                    {
                        EcsEventBus.Publish(GameplayEventType.DealDamage, senderEntity, new DealDamageEventArgs(otherEntity, damage, args.SenderStats, senderEntity));
                    }
                    break;
                default:
                    break;
            }
        }
        */
    }
}
