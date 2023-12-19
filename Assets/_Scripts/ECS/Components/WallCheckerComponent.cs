using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct WallCheckerComponent
{
    public Transform WallChecker;
    public float Radius;
    public LayerMask WallMask;
}
