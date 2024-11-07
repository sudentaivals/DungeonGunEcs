using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Create action with condition")]
public class ActionWithCondition : GameAction
{
    [SerializeField] private BaseGameCondition _condition;
    [SerializeField] private GameAction _action;

    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var conditionValid = _condition == null ? true : _condition.CheckCondition(senderEntity, takerEntity, conditionAndActionArgs);
        if (conditionValid) _action.Action(senderEntity, takerEntity, conditionAndActionArgs);
    }
}
