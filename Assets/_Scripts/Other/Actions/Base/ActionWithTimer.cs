using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Create action with timer")]
public class ActionWithTimer : GameAction
{
    [SerializeField] float _timer;
    [SerializeField] GameAction _action;

    public override void Action(int senderEntity, int? takerEntity)
    {
        var args = EventArgsObjectPool.GetArgs<RegisterTimedActionEventArgs>();
        args.Timer = _timer;
        args.Action = _action;
        args.TakerEntity = takerEntity;
        EcsEventBus.Publish(GameplayEventType.RegisterTimedAction, senderEntity, args);
    }
}
