using System;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;

[Serializable]
public struct ObjectPoolHandlerComponent
{
    public GameObject GameObjectReference;

    public List<GameAction> ActionsOnReturnToPool;

}
