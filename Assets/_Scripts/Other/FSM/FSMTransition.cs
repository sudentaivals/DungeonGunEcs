using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/FSM/Sequence")]
public class FSMTransition : ScriptableObject
{
    public NpcState State;
    public BaseGameCondition Condition;

    public bool AllStateConditionsValid(int senderEntity)
    {
        if(Condition == null) return true;
        return Condition.CheckCondition(senderEntity, null);
    }

}
