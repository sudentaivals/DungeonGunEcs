using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GizmosHandler : MonoBehaviour
{
    [SerializeField] List<CircleDrawHandler> _circles;
    [SerializeField] List<RectangleDrawHandler> _rectangles;
    private void OnDrawGizmos()
    {
        if(_circles != null)
        {
            foreach (var circle in _circles)
            {
                Gizmos.color = circle.Color;
                Gizmos.DrawWireSphere(transform.position + circle.Offset, circle.Radius);
            }
        }
        if(_rectangles != null)
        {
            foreach (var rectangle in _rectangles)
            {
                Gizmos.color = rectangle.Color;
                Gizmos.DrawWireCube(transform.position + rectangle.Offset, rectangle.Size);
            }
        }
    }
}

[Serializable]
public struct CircleDrawHandler
{
    public Vector3 Offset;
    public float Radius;
    public Color Color;
}

[Serializable]
public struct RectangleDrawHandler
{
    public Vector2 Size;

    public Vector3 Offset;
    
    public Color Color;
}
