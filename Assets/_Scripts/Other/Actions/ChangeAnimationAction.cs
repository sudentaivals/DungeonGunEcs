using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Change animation")]
public class ChangeAnimationAction : GameAction
{
    [SerializeField] int _newAnimationState;
    [SerializeField] float _lockTime;
    [SerializeField] float _targetAnimSpeed;
    [SerializeField] bool _changingSpeed;
    public override void Action(int senderEntity, int? takerEntity)
    {
        var animArgs = EventArgsObjectPool.GetArgs<PlayAnimationEventArgs>();
        animArgs.AnimationId = _newAnimationState;
        animArgs.LockTime = _lockTime;
        animArgs.TargetAnimationSpeed = _targetAnimSpeed;
        animArgs.ChangeAnimationSpeed = _changingSpeed;
        EcsEventBus.Publish(GameplayEventType.ChangeAnimation, senderEntity, animArgs);
    }
}