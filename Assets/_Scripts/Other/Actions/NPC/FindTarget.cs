using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/NPC/Find target")]
public class FindTarget : GameAction
{
    [SerializeField] List<BaseGameCondition> _targetConditions;
    public override void Action(int senderEntity, int? takerEntity)
    {
        var statsPool = EcsStart.World.GetPool<GlobalStatsComponent>();
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        var targetPool = EcsStart.World.GetPool<NpcTargetComponent>();
        ref var senderStats = ref statsPool.Get(senderEntity);
        ref var senderNpcTarget = ref targetPool.Get(senderEntity);
        ref var senderTransform = ref transformPool.Get(senderEntity);

        var entities = PhysicsHelper.GetAllEntitiesInRadius(senderTransform.Transform.position, senderNpcTarget.TargetRadius);
        //Debug.Log($"Entities: {entities.Count}");
        if (entities.Count == 0) return;
        var filteredEntities = entities.Where(a => _targetConditions.Select(b => b.CheckCondition(senderEntity, a)).All(b => b == true)).ToList();
        //Debug.Log($"Filtered: {filteredEntities.Count}");
        if (filteredEntities.Count == 0) return;
        var senderPos = senderTransform.Transform.position;
        var closestEntity = filteredEntities.OrderBy(a => (transformPool.Get(a).Transform.position - senderPos).magnitude).First();
        ref var targetTransform = ref transformPool.Get(closestEntity);
        //senderNpcTarget.Target = targetTransform.Transform;
    }
}
