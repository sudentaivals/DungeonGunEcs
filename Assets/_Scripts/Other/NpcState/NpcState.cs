using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Npc state")]
public class NpcState : ScriptableObject
{
    [SerializeField] int _id;
    [SerializeField] List<BaseGameCondition> _endConditions;
    [SerializeField] List<GameAction> _actionsOnStateStart;
    [SerializeField] List<GameAction> _actionsOnStateEnd;
    [SerializeField] List<GameAction> _actionsOnStateUpdate;

    public int Id => _id;


    public bool AllEndConditionsValid(int senderEntity)
    {
        if (_endConditions.Count == 0) return false;
        var conditionsAsBool = _endConditions.Select(a => a.CheckCondition(senderEntity, null));
        return conditionsAsBool.All(a => a == true);
    }

    public void PerformActionsOnStart(int senderEntity)
    {
        foreach (var action in _actionsOnStateStart)
        {
            action.Action(senderEntity, null);
        }
    }

    public void PerformActionsOnEnd(int senderEntity)
    {
        foreach (var action in _actionsOnStateEnd)
        {
            action.Action(senderEntity, null);
        }
    }
    public void PerformActionsOnUpdate(int senderEntity)
    {
        foreach (var action in _actionsOnStateUpdate)
        {
            action.Action(senderEntity, null);
        }
    }


}
