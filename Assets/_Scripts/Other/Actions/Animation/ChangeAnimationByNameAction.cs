using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Animation/Change animation by name")]
public class ChangeAnimationByNameAction : GameAction
{
    [SerializeField] string _newAnimationName;
    [SerializeField] float _lockTime;
    [SerializeField] float _targetAnimSpeed;
    [SerializeField] bool _changingSpeed;
    [SerializeField] bool _ignoreExitActions;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var animArgs = EventArgsObjectPool.GetArgs<PlayAnimationByNameEventArgs>();
        animArgs.AnimationName = _newAnimationName;
        animArgs.LockTime = _lockTime;
        animArgs.TargetAnimationSpeed = _targetAnimSpeed;
        animArgs.ChangeAnimationSpeed = _changingSpeed;
        animArgs.IgnoreExitActions = _ignoreExitActions;
        EcsEventBus.Publish(GameplayEventType.ChangeAnimationByName, senderEntity, animArgs);
    }
}
