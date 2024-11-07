using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
public class ObjectPoolHandler : MonoBehaviour
{
    [SerializeField] int _poolId;
    public int PoolId => _poolId;
    [SerializeField] private SerializableGuid _serializableGuid;
    public Guid Guid => _serializableGuid.Guid;
}
