using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct BulletStatsComponent
{
    public bool IsActivated;
    //fly distance
    public float MaxFlyDistance;
    public float CurrentFlyDistance;
    public Vector3 OldPosition;

    public GameAction ActionsOnReachMaxFlyDistance;
}


