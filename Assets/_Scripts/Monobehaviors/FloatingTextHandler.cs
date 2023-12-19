using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextHandler : MonoBehaviour
{
    public Vector3 SpawnPosition { get; set; }
    public string Text { get; set; }

    public Color Color { get; set; }
    public float Lifetime { get; set; }
    public Vector2 Direction { get; set; }
    public float Velocity { get; set; }
}
