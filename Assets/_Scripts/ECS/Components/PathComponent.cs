using System;
using System.Collections.Generic;
using CustomAstar;

[Serializable]
public struct PathComponent
{
    public Stack<Node> Nodes;
    public float PathCalculationDelay;

    public float CurrentDelay;
    public float NodeRemoveEpsilon;
}
