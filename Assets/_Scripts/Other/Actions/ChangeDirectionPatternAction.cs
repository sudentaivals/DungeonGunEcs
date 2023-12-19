using AYellowpaper;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Change direction pattern")]
public class ChangeDirectionPatternAction : GameAction
{
    [SerializeField] InterfaceReference<IMovementDirection, ScriptableObject> _newMovementDirectionPattern;
    public override void Action(int senderEntity, int? takerEntity)
    {
        var args = EventArgsObjectPool.GetArgs<ChangeMovementDirectionPatternEventArgs>();
        args.NewMovementDirectionPattern = _newMovementDirectionPattern;
        EcsEventBus.Publish(GameplayEventType.ChangeDirectionPattern, senderEntity, args);
    }
}
