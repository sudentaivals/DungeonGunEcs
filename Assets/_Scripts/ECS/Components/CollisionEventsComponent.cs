using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CollisionEventsComponent
{
    public BaseGameCondition Condition;
    public GameAction Action;
}
