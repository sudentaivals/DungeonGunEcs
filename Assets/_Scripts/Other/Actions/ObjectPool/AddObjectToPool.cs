using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Add object to pool")]
public class AddObjectToPool : GameAction
{
    [SerializeField] int _poolId;
    public override void Action(int senderEntity, int? takerEntity)
    {
        var args = EventArgsObjectPool.GetArgs<ReturnObjectToPoolEventArgs>();
        args.PoolId = _poolId;
        EcsEventBus.Publish(GameplayEventType.ReturnObjectToPool, senderEntity, args);
    }
}
