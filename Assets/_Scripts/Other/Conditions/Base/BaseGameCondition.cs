using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameCondition : ScriptableObject
{
    public virtual bool CheckCondition(int senderEntity, int? takerEntity)
    {
        return true;
    }
}
