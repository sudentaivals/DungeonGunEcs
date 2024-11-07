using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UsableObjectComponent
{
    public bool IsSelectable;
    public GameObject _actionsTarget;
    public bool UseActionsTakerIsTarget;
    public GameAction ActionOnSelect;
    public GameAction ActionOnDeselect;
    public GameAction ActionOnUse;
}
