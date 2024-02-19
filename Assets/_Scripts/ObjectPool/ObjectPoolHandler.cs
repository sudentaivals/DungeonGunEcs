using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;

public class ObjectPoolHandler : MonoBehaviour
{
    [SerializeField] int _poolId;
    public int PoolId => _poolId;
    //[RequireInterface(typeof(IObjectPoolStatsRestore))]
    //[SerializeField] List<ScriptableObject> _componentsResetList;

    //public List<ScriptableObject> ComponentResetList => _componentsResetList;
}
