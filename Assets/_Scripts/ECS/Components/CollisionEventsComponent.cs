using System;

[Serializable]
public struct CollisionEventsComponent
{
    public BaseGameCondition Condition;
    public GameAction Action;
    public bool Triggered;
    public bool IsInfinite;
    public int BaseNumberOfCollisions;
    public int CurrentNumberOfCollisions;
}
