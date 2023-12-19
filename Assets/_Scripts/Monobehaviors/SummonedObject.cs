using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedObject : MonoBehaviour
{
    private int _masterEntity = -1;

    private bool _isMasterEntitySet = false;

    public int MasterEntity => _masterEntity;

    public bool IsMasterEntitySet => _isMasterEntitySet;
    public void SetMasterEntity(int masterEntity)
    {
        _masterEntity = masterEntity;
    }

    public void SetMasterEntityState(bool newState)
    {
        _isMasterEntitySet = newState;
    }
}
