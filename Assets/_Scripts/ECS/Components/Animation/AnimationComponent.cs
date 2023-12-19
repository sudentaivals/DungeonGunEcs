using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AnimationComponent
{
    public bool IsActivated;
    public Animator Animator;

    public float LockTime;

    public string MoveAnimationName => "Move";
    public string IdleAnimationName => "Idle";

    public string AttackAnimationName => "Attack";

    public int CurrentState;

    public int Idle;

    public int Move;

    public int Attack;
}
