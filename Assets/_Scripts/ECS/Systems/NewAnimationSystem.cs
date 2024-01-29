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
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<NewAnimationComponent>()
                       .Exc<PooledObjectTag>()
                       .End();
        _newAnimationPool = world.GetPool<NewAnimationComponent>();
        EcsEventBus.Subscribe(GameplayEventType.ChangeAnimation, ChangeAnimation);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            SetNewAnimationId(entity);
            SetAnimationTransition(entity);
            //TriggerAnimationExit(entity);
            /*
            ref var newAnimationComponent = ref _newAnimationPool.Get(entity);
            if(Time.time < newAnimationComponent.LockedTill) continue;
            //find new state
            var newState = newAnimationComponent.AnimationStates.OrderBy(state => state.Priority)
                                                                .Where(state => state.CheckStartCondition(entity))
                                                                .FirstOrDefault();
            if(newState == null) continue;

            if(newState.Id == newAnimationComponent.CurrentStateId)
            {
                if((int)newAnimationComponent.AnimancerComponent.States.Current.NormalizedTime % 1 == 0) continue;
                newAnimationComponent.AnimancerComponent.States.Current.NormalizedTime = 0f;
            }
            else
            {
                //exit actions
                int oldStateId = newAnimationComponent.CurrentStateId;
                var oldState = newAnimationComponent.AnimationStates.FirstOrDefault(x => x.Id == oldStateId);
                if(oldState != null)
                {
                    newAnimationComponent.AnimancerComponent.States.Current.Speed = BASE_SPEED;// newAnimationComponent.AnimancerComponent.States.Current.Length;
                    oldState.OnAnimationEnd(entity);
                }
                //new state assign and actions
                newAnimationComponent.CurrentStateId = newState.Id;
                newAnimationComponent.AnimancerComponent.Play(newState.Animation);
                newState.OnAnimationStart(entity);
            }
            */
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
            if((int)newAnimationComponent.AnimancerComponent.States.Current.NormalizedTime % 1 == 0) return;
            newAnimationComponent.AnimancerComponent.States.Current.NormalizedTime = 0f;
        }
        else
        {
            newAnimationComponent.OldStateId = newAnimationComponent.CurrentStateId;
            newAnimationComponent.CurrentStateId = newAnimationComponent.NewStateId;

            //start actions
            var currentStateId = newAnimationComponent.CurrentStateId;
            var currentState = newAnimationComponent.AnimationStates.FirstOrDefault(state => state.Id == currentStateId);
            //anim speed diff
            if(newAnimationComponent.AnimancerComponent.States.Count > 0) newAnimationComponent.AnimancerComponent.States.Current.Speed = BASE_SPEED;
            newAnimationComponent.AnimancerComponent.Play(currentState.Animation);
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
                        oldState.OnAnimationEnd(entity);
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



        /*
        var animantionState = newAnimationComponent.AnimationStates.FirstOrDefault(x => x.Id == changeAnimArgs.AnimationId);
        if(animantionState == null) return;
        //old state exit and length reset
        int oldStateId = newAnimationComponent.CurrentStateId;
        var oldState = newAnimationComponent.AnimationStates.FirstOrDefault(x => x.Id == oldStateId);
        if(oldState != null)
        {
            newAnimationComponent.AnimancerComponent.States.Current.Speed = BASE_SPEED;// newAnimationComponent.AnimancerComponent.States.Current.Length;
            if(!changeAnimArgs.IgnoreExitActions) oldState.OnAnimationEnd(sender);
        }

        //new state assign
        newAnimationComponent.CurrentStateId = animantionState.Id;
        newAnimationComponent.AnimancerComponent.Play(animantionState.Animation);

        if(changeAnimArgs.ChangeAnimationSpeed)
        {
            float animationBaseSpeed = newAnimationComponent.AnimancerComponent.States.Current.Length;
            float animationSpeed = animationBaseSpeed / changeAnimArgs.TargetAnimationSpeed;
            newAnimationComponent.AnimancerComponent.States.Current.Speed = animationSpeed;
        }
        
        animantionState.OnAnimationStart(sender);

        newAnimationComponent.LockedTill = Time.time + changeAnimArgs.LockTime;
        */
    }
}
