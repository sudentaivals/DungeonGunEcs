using System;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

public class NewAnimationSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<NewAnimationComponent> _newAnimationPool;

    private const float BASE_SPEED = 1.0f;

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.ChangeAnimation, ChangeAnimation);
        EcsEventBus.Unsubscribe(GameplayEventType.ChangeAnimationByName, ChangeAnimationByName);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<NewAnimationComponent>()
                       .Exc<PooledObjectTag>()
                       .End();
        _newAnimationPool = world.GetPool<NewAnimationComponent>();
        EcsEventBus.Subscribe(GameplayEventType.ChangeAnimation, ChangeAnimation);
        EcsEventBus.Subscribe(GameplayEventType.ChangeAnimationByName, ChangeAnimationByName);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            SetNewAnimationId(entity);
            SetAnimationTransition(entity);
        }
    }

    private void TriggerAnimationExit(int entity)
    {
        throw new NotImplementedException();
    }

    private void SetAnimationTransition(int entity)
    {
        ref var newAnimationComponent = ref _newAnimationPool.Get(entity);
        if(newAnimationComponent.NewStateId == newAnimationComponent.CurrentStateId)
        {
            //Debug.Log((int)newAnimationComponent.AnimancerComponent.States.Current.NormalizedTime % 1);
            //Debug.Log(newAnimationComponent.AnimancerComponent.States.Current.NormalizedTime);
            if(newAnimationComponent.AnimancerComponent.States.Current == null) return;
            if(newAnimationComponent.AnimancerComponent.States.Current.NormalizedTime < 1) return;
            var currentAnimancerState = newAnimationComponent.AnimancerComponent.States.Current;
            var currentStateId = newAnimationComponent.CurrentStateId;
            var currentState = newAnimationComponent.AnimationStates.FirstOrDefault(state => state.Id == currentStateId);
            if(currentState.IsRepeatable)
            {
                newAnimationComponent.AnimancerComponent.States.Current.NormalizedTime = 0f;
            }
            currentState.OnAnimationEnd(entity);
        }
        else
        {
            newAnimationComponent.OldStateId = newAnimationComponent.CurrentStateId;
            newAnimationComponent.CurrentStateId = newAnimationComponent.NewStateId;
            //start actions
            var currentStateId = newAnimationComponent.CurrentStateId;
            var currentState = newAnimationComponent.AnimationStates.FirstOrDefault(state => state.Id == currentStateId);
            //anim speed diff
            if(newAnimationComponent.AnimancerComponent.States.Count > 0)
                if(newAnimationComponent.AnimancerComponent.States.Current != null) newAnimationComponent.AnimancerComponent.States.Current.Speed = BASE_SPEED;
            newAnimationComponent.AnimancerComponent.Play(currentState.Animation);
            //newAnimationComponent.AnimancerComponent.States.Current.IsLooping = currentState.IsLooping;
            if(newAnimationComponent.ChangeSpeed)
            {
                float animationBaseSpeed = newAnimationComponent.AnimancerComponent.States.Current.Length;
                float animationSpeed = animationBaseSpeed / newAnimationComponent.DesireAnimationSpeed;
                newAnimationComponent.AnimancerComponent.States.Current.Speed = animationSpeed;
                newAnimationComponent.ChangeSpeed = false;
            }
            currentState.OnAnimationStart(entity);
            //exit actions
            var oldStateId = newAnimationComponent.OldStateId;
            var oldState = newAnimationComponent.AnimationStates.FirstOrDefault(state => state.Id == oldStateId);
            if(oldState != null)
            {
                if(newAnimationComponent.TriggerExitActions == true)
                {
                    oldState.OnAnimationExit(entity);
                }
            }
            newAnimationComponent.TriggerExitActions = true;
        }
    }

    private void SetNewAnimationId(int entity)
    {
        ref var newAnimationComponent = ref _newAnimationPool.Get(entity);
        int newState = -1;
        if(Time.time < newAnimationComponent.LockedTill)
        {
            newState = newAnimationComponent.LockedState;
        }
        else
        {
            var state = newAnimationComponent.AnimationStates.OrderBy(state => state.Priority)
                                                             .FirstOrDefault(state => state.CheckStartCondition(entity));
            if(state == null) return;
            newState = state.Id;
        }
        newAnimationComponent.NewStateId = newState;
    }



    private void ChangeAnimation(int sender, EventArgs args)
    {
        var changeAnimArgs = args as PlayAnimationEventArgs;
        ref var newAnimationComponent = ref _newAnimationPool.Get(sender);
        newAnimationComponent.LockedTill = Time.time + changeAnimArgs.LockTime;
        newAnimationComponent.LockedState = changeAnimArgs.AnimationId;
        newAnimationComponent.ChangeSpeed = changeAnimArgs.ChangeAnimationSpeed;
        newAnimationComponent.DesireAnimationSpeed = changeAnimArgs.TargetAnimationSpeed;
        newAnimationComponent.TriggerExitActions = !changeAnimArgs.IgnoreExitActions;
    }

    private void ChangeAnimationByName(int sender, EventArgs args)
    {
        var changeAnimArgs = args as PlayAnimationByNameEventArgs;
        ref var newAnimationComponent = ref _newAnimationPool.Get(sender);
        BaseAnimationState animationState = null;
        foreach(var state in newAnimationComponent.AnimationStates)
        {
            if(state.Name == changeAnimArgs.AnimationName)
            {
                animationState = state;
                break;
            }
        }
        if(animationState == null) return;

        newAnimationComponent.LockedState = animationState.Id;
        newAnimationComponent.LockedTill = Time.time + changeAnimArgs.LockTime;
        newAnimationComponent.ChangeSpeed = changeAnimArgs.ChangeAnimationSpeed;
        newAnimationComponent.DesireAnimationSpeed = changeAnimArgs.TargetAnimationSpeed;
        newAnimationComponent.TriggerExitActions = !changeAnimArgs.IgnoreExitActions;

    }
}
