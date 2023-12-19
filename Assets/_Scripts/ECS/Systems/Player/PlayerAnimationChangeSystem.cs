using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationChangeSystem : IEcsRunSystem
{

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var ecsFilter = world.Filter<PlayerStateComponent>().End();
        var statePool = world.GetPool<PlayerStateComponent>();

        foreach (int entity in ecsFilter)
        {
            var state = statePool.Get(entity);
            Transition(ref state);
        }

    }

    private void Transition(ref PlayerStateComponent stateComponent)
    {
        var currentClip = stateComponent.Animator.GetCurrentAnimatorClipInfo(0);
        if (currentClip[0].clip.name == stateComponent.CurrentState) return;
        stateComponent.Animator.Play(stateComponent.CurrentState);
    }


}
