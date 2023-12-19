using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Create action pack")]
public class ActionPackage : GameAction
{
    [SerializeField] BaseGameCondition _condition;
    [SerializeField] List<GameAction> _actions;

    public override void Action(int senderEntity, int? takerEntity)
    {
        var conditionsValid = _condition == null ? true : _condition.CheckCondition(senderEntity, takerEntity);
        if (!conditionsValid) return;
        foreach (var gameAction in _actions)
        {
            gameAction.Action(senderEntity, takerEntity);
        }
    }
}
