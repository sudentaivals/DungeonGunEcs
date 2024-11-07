using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/NPC/Npc animation is finished")]
public class CurrentNpcAnimationIsFinished : BaseGameCondition
{

    [SerializeField] bool _isFinished;

    private readonly int _animationLayer = 0;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var world = EcsStart.World;
        var animationPool = world.GetPool<AnimationComponent>();
        ref var animationComponent = ref animationPool.Get(senderEntity);
        
        if(_isFinished) return animationComponent.Animator.GetCurrentAnimatorStateInfo(_animationLayer).normalizedTime >= 1;
        else return animationComponent.Animator.GetCurrentAnimatorStateInfo(_animationLayer).normalizedTime < 1;
    }


}
