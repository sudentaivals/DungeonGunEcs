using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct PlayerStateComponent
{
    public string IdleState => "Idle";
    public string RunState => "Run";
    public string MoveState => "Move";
    public string DeathState => "Death";
    public string RollState => "Roll";

    public string CurrentState;

    public Animator Animator;

}
