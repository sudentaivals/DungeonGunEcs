using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Conditions/Current animation id is")]
public class CurrentAnimationIdIs : BaseGameCondition
{
    [SerializeField] int _animationId;
    [SerializeField] bool _isReverse;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var newAnimationPool = EcsStart.World.GetPool<NewAnimationComponent>();
        ref var newAnimation = ref newAnimationPool.Get(senderEntity);
        if(_isReverse) return newAnimation.CurrentStateId != _animationId;
        else return newAnimation.CurrentStateId == _animationId;
    }
}
