using System;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Animation/Change animation by id")]
public class ChangeAnimationAction : GameAction
{
    [SerializeField] private Guid _animationGuid;
    [SerializeField] int _newAnimationState;
    [SerializeField] float _lockTime;
    [SerializeField] float _targetAnimSpeed;
    [SerializeField] bool _changingSpeed;
    [SerializeField] bool _ignoreExitActions;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var animArgs = EventArgsObjectPool.GetArgs<PlayAnimationEventArgs>();
        animArgs.AnimationId = _newAnimationState;
        animArgs.LockTime = _lockTime;
        animArgs.TargetAnimationSpeed = _targetAnimSpeed;
        animArgs.ChangeAnimationSpeed = _changingSpeed;
        animArgs.IgnoreExitActions = _ignoreExitActions;
        EcsEventBus.Publish(GameplayEventType.ChangeAnimation, senderEntity, animArgs);
    }
}
