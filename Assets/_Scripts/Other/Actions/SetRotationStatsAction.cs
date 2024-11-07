using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Set rotation stats")]
public class SetRotationStatsAction : GameAction
{
    [SerializeField] bool _isClockwise;
    [SerializeField] float _rotationSpeed;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var setRotationArgs = EventArgsObjectPool.GetArgs<SetRotationStatsEventArgs>();
        setRotationArgs.IsClockwise = _isClockwise;
        setRotationArgs.RotationSpeed = _rotationSpeed;
        EcsEventBus.Publish(GameplayEventType.SetRotationStats, senderEntity, setRotationArgs);
    }
}
