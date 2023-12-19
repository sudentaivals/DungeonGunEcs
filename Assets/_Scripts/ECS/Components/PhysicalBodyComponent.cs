using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PhysicalBodyComponent
{
    public Rigidbody2D RigidBody;
    public Collider2D PhysicalBody;

    public float BodyRadius;
}
