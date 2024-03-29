using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerInputComponent
{
    //shoot
    public bool IsShooting;
    public bool IsAutomaticShooting;

    public bool IsAlternateShooting;
    //move
    public float HorizontalMovement;
    public float VerticalMovement;
    //roll
    public bool RollPressed;

    //usable items
    public bool UseObject;

    public bool UsableObjectIsSwapped;
}
