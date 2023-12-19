using System;
using TMPro;
using UnityEngine;
[Serializable]
public struct FloatingTextComponent
{
    public TMP_Text Text;
    public RectTransform Transform;
    public float CurrentLifetime;
    public float MaxLifetime;
    public bool IsActivated;
    public Vector2 Direction;
    public float Velocity;
}
