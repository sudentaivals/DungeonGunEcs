using System;
using System.Collections.Generic;
using Animancer;

[Serializable]
public struct NewAnimationComponent
{
    public AnimancerComponent AnimancerComponent;
    public List<BaseAnimationState> AnimationStates;

    public int LockedState;

    public int OldStateId;

    public int NewStateId;

    public bool ChangeSpeed;

    public float DesireAnimationSpeed;

    public bool TriggerExitActions;
    public float LockedTill;
    public int CurrentStateId;

}
