using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/Pack/Selector")]
public sealed class SelectorCondition : BaseGameCondition
{
    [SerializeField] private List<BaseGameCondition> _conditions;

    public sealed override bool CheckCondition(int senderEntity, int? takerEntity)
    {
        if(_conditions == null || _conditions.Count == 0) return true;

        foreach (var condition in _conditions)
        {
            if (condition.CheckCondition(senderEntity, takerEntity)) return true;
        }

        return false;
    }
}
