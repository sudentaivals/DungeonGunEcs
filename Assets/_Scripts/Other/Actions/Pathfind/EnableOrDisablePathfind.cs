using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Enable or disable pathfinding")]
public class EnableOrDisablePathfind : GameAction
{
    [SerializeField] bool _targetIsSender;
    [SerializeField] bool _enable;
    public override void Action(int senderEntity, int? takerEntity)
    {
        int entity = senderEntity;
        if(!_targetIsSender)
        {
            if(takerEntity == null) return;
            entity = takerEntity.Value;
        }
        var args = EventArgsObjectPool.GetArgs<ChangePathfindingStatusEventArgs>();
        args.IsPathfindingActive = _enable;
        EcsEventBus.Publish(GameplayEventType.SetPathfindingStatus, entity, args);
    }
}
