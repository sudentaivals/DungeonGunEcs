using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NpcTargetComponent
{
    public int TargetEntity;
    public float TargetRadius;
    public float FindTargetDelay;
    public bool IsTargetFound;
    public BaseGameCondition TargetCondition;
}
