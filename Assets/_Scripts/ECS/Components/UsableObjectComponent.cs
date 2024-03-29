using System;
using System.Collections.Generic;

[Serializable]
public struct UsableObjectComponent
{
    public bool IsSelectable;
    public GameAction ActionOnSelect;
    public GameAction ActionOnDeselect;
    public GameAction ActionOnUse;
}
