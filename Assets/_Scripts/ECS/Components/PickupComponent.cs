using System;
using System.Collections.Generic;

[Serializable]
public struct PickupComponent
{
    public float PickupRadius;
    public List<int> NearbyUsableObjects;
    public int CurrentSelectedUsableObject;
    public bool UsableObjectIsSelected;
}
