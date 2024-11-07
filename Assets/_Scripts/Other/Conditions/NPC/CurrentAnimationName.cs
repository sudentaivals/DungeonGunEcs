using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/NPC/Current Animation Name")]

public class CurrentAnimationName : BaseGameCondition
{
    [SerializeField] string _animationName;
    [SerializeField] bool _isReverse;

    private readonly int _animationLayer = 0;

    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var world = EcsStart.World;
        var animationPool = world.GetPool<AnimationComponent>();
        ref var animationComponent = ref animationPool.Get(senderEntity);

        var currentClip = animationComponent.Animator.GetCurrentAnimatorClipInfo(_animationLayer);
        //Debug.Log(currentClip[0].clip.name);
        if(_isReverse) return currentClip[0].clip.name != _animationName;
        else return currentClip[0].clip.name == _animationName;
        //if (currentClip[0].clip.name == _animationName) return true;
        //else return false;

    }
}
