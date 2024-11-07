using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Conditions/Objects in range condition")]
public class ThereIsObjectsInRange : BaseGameCondition
{
    [SerializeField] private BaseGameCondition _filter;
    [SerializeField] private float _radius;
    [SerializeField] private Vector2 _offset;

    private List<int> GetTargets(int senderEntity)
    {
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        var pos = transformPool.Get(senderEntity).Transform.position + (Vector3)(Mathf.Sign(transformPool.Get(senderEntity).Transform.localScale.x) * _offset);
        var entetiesInRange = PhysicsHelper.GetAllEntitiesInRadius(pos, _radius);
        //Debug.Log($"{entetiesInRange.Count}");
        var filteredEntities = entetiesInRange.Where(a => _filter.CheckCondition(senderEntity, a) == true).ToList();
        //Debug.Log($"{filteredEntities.Count}");
        return filteredEntities;
    }
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var targets = GetTargets(senderEntity);
        return targets.Count > 0;
    }
}
