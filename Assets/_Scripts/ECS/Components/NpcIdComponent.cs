using System;
using UnityEngine;

[Serializable]
public struct NpcIdComponent
{
    public GameObject GameObjectReference;
    public SerializableGuid SerializableGuid;
    public int Id;
    public string Name;
    public string Description;
}
