using System;
using System.Collections.Generic;
using Animancer;

[Serializable]
public struct NewAnimationComponent
{
    public AnimancerComponent AnimancerComponent;
    public List<BaseAnimationState> AnimationStates;
    public float LockedTill;
    public int CurrentStateId;

}
