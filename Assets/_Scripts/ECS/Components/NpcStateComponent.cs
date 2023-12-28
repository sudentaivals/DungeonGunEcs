using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NpcStateComponent
{
    public FSM FSM;
    public int RunningStateId;
    public float UpateDelay;
    public float CurrentUpdateDelay;
}
