using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/FSM/Sequence")]
public class FSMTransition : ScriptableObject
{
    public NpcState State;
    public List<BaseGameCondition> Conditions;

    public bool AllStateConditionsValid(int senderEntity)
    {
        if (Conditions.Count == 0) return true;
        var conditionsAsBool = Conditions.Select(a => a.CheckCondition(senderEntity, null));
        return conditionsAsBool.All(a => a == true);
    }

}
