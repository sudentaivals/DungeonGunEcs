using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Create action with timer")]
public class ActionWithTimer : GameAction
{
    [SerializeField] float _timer;
    [SerializeField] GameAction _action;

    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var args = EventArgsObjectPool.GetArgs<RegisterTimedActionEventArgs>();
        args.Timer = _timer;
        args.Action = _action;
        args.TakerEntity = takerEntity;
        args.Args = conditionAndActionArgs;
        EcsEventBus.Publish(GameplayEventType.RegisterTimedAction, senderEntity, args);
    }

    private void OnValidate()
    {
        if(_timer < 0) _timer = 0;
    }
}
