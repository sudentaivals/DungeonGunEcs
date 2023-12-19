using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Area game action")]
public class AreaGameAction : GameAction
{
    [SerializeField] protected float _radius;
    [SerializeField] List<BaseGameCondition> _conditions;
    [SerializeField] List<GameAction> _actions;
    [SerializeField] Vector3 _offset;

    private List<int> GetTargets(int senderEntity)
    {
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        var pos = transformPool.Get(senderEntity).Transform.position + (Mathf.Sign(transformPool.Get(senderEntity).Transform.localScale.x) * _offset);
        var entetiesInRange = PhysicsHelper.GetAllEntitiesInRadius(pos, _radius);
        //Debug.Log($"{entetiesInRange.Count}");
        var filteredEntities = entetiesInRange.Where(a => _conditions.Select(b => b.CheckCondition(senderEntity, a)).All(b => b == true)).ToList();
        //Debug.Log($"{filteredEntities.Count}");
        return filteredEntities;
    }

    public sealed override void Action(int senderEntity, int? takerEntity)
    {
        var targets = GetTargets(senderEntity);
        foreach (int entity in targets)
        {
            foreach (var action in _actions)
            {
                action.Action(senderEntity, entity);
            }
        }
    }
}

