using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Set special movement stats")]
public class SetSpecialMovementStatsAction : GameAction
{
    public override void Action(int senderEntity, int? takerEntity)
    {
        EcsEventBus.Publish(GameplayEventType.ActivateSpecialMovement, senderEntity, null);
    }
}
