using System;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;

[Serializable]
public struct MovementStatsComponent
{
    public InterfaceReference<IMovementDirection, ScriptableObject> MovementDirectionPattern;
    public Vector2 Movement;
    public Vector2 MovementDirection;
    public List<float> MovementSpeedBonus;
    public float MovementSpeed;
    public float Acceleration;
    public float Deceleration;
    public bool IsMovementAvailable;
}
