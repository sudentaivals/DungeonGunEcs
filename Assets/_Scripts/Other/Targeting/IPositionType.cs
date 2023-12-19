using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IPositionType
{
    public Vector2 GetPosition(int sender, int? taker);
}
