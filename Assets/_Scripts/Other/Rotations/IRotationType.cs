using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRotationType
{
    public Quaternion GetRotation(int sender, int? taker);
}
