using Leopotam.EcsLite;
using System;
using UnityEngine;

public class AnimationChangeSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{

    private EcsFilter _filter;
    private EcsPool<MovementStatsComponent> _movementStatsPool;

    private EcsPool<AnimationComponent> _animationPool;


    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _movementStatsPool = null;
        _animationPool = null;

        EcsEventBus.Unsubscribe(GameplayEventType.ChangeAnimation, PlayAnimation);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<AnimationComponent>().Exc<PooledObjectTag>().End();

        _movementStatsPool = world.GetPool<MovementStatsComponent>();
        _animationPool = world.GetPool<AnimationComponent>();

        EcsEventBus.Subscribe(GameplayEventType.ChangeAnimation, PlayAnimation);
    }

    private void PlayAnimation(int entity, EventArgs args)
    {
        var playAnimArgs = args as PlayAnimationEventArgs;
        ref var animationComponent = ref _animationPool.Get(entity);
        animationComponent.CurrentState = playAnimArgs.AnimationId;
        animationComponent.LockTime = Time.time + playAnimArgs.LockTime;
        Transition(entity, animationComponent.CurrentState);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var animationComponent = ref _animationPool.Get(entity);
            if(!animationComponent.IsActivated) ActivateAnimator(entity);
            int newState = SelectNewState(entity);
            if(newState == animationComponent.CurrentState) continue;
            Transition(entity, newState);
        }
    }

    private void ActivateAnimator(int entity)
    {
        ref var animationComponent = ref _animationPool.Get(entity);
        animationComponent.Attack = Animator.StringToHash(animationComponent.AttackAnimationName);
        animationComponent.Idle = Animator.StringToHash(animationComponent.IdleAnimationName);
        animationComponent.Move = Animator.StringToHash(animationComponent.MoveAnimationName);
        animationComponent.IsActivated = true;
    }

    private int SelectNewState(int entity)
    {
        ref var statsComponent = ref _movementStatsPool.Get(entity);
        ref var animationComponent = ref _animationPool.Get(entity);
        if(Time.time < animationComponent.LockTime) return animationComponent.CurrentState;
        if (statsComponent.Movement.magnitude >= float.Epsilon) return animationComponent.Move;
        return animationComponent.Idle;
    }

    private void Transition(int entity, int newAnimationState)
    {
        ref var animationComponent = ref _animationPool.Get(entity);
        animationComponent.Animator.CrossFade(newAnimationState, 0, 0);
    }
}
